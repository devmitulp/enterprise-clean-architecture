using Application.Common.Helpers;
using Application.Common.Interfaces.Base;
using Application.Common.Interfaces.JwtToken;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Auth;
using Application.Common.Models;
using Application.Common.Settings;
using Application.Features.Auth;
using Application.Features.Auth.DTOs;
using Domain.Entities.Users;
using Domain.Entities.UserSessions;
using Infrastructure.Services.Common.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Exceptions;
using Shared.Results;

namespace Infrastructure.Services.Auth
{
    public class AuthService : ApplicationBaseService, IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserSession> _sessionRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ITotpService _totpService;
        private readonly ILogger<AuthService> _logger;

        private readonly JwtSettings _jwtSettings;
        private readonly MfaSettings _mfaSettings;

        public AuthService(
            IServiceContext context,
            IRepository<User> userRepository,
            IRepository<UserSession> sessionRepository,
            IJwtTokenService jwtTokenService,
            IPasswordHelper passwordHelper,
            ITotpService totpService,
            IOptions<JwtSettings> jwtSettings,
            IOptions<MfaSettings> mfaSettings,
            ILogger<AuthService> logger) : base(context)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHelper = passwordHelper;
            _totpService = totpService;
            _logger = logger;

            _jwtSettings = jwtSettings.Value;
            _mfaSettings = mfaSettings.Value;
        }

        public async Task<LoginResponseDto> LoginAsync(
            LoginRequestDto request,
            string? userAgent,
            string? timeZone,
            CancellationToken ct = default)
        {
            var user = await _userRepository
                        .AsQueryable()
                        .Include(x => x.Employee)
                        .ThenInclude(x => x.JobTitle)
                        .Where(x => x.Employee.Email == request.Email && x.IsActive)
                        .FirstOrDefaultAsync(ct);

            if (user is null)
                throw new UnauthorizedException(Localization.L("InvalidCredentials"));

            var isValidPassword = _passwordHelper.VerifyPassword(
                user,
                request.Password,
                user.PasswordHash);

            if (!isValidPassword)
                throw new UnauthorizedException(Localization.L("InvalidCredentials"));

            // Check if user has MFA set up or needs setup
            if (!string.IsNullOrEmpty(user.MfaSecret))
            {
                // Check if user is currently locked out from MFA
                if (user.MfaLockedUntilUtc.HasValue && user.MfaLockedUntilUtc.Value > DateTime.UtcNow)
                {
                    _logger.LogWarning("Login blocked: User {UserId} is currently locked out from MFA until {LockedUntilUtc}.", user.Id, user.MfaLockedUntilUtc);
                    throw new UnauthorizedException(Localization.L("MfaLockedOut"));
                }

                // Return MFA challenge for existing setup
                return new LoginResponseDto
                {
                    RequiresMfa = true,
                    IsMfaSetupRequired = false,
                    MfaToken = _jwtTokenService.GenerateMfaToken(user)
                };
            }

            // User has not set up MFA yet -> generate TOTP setup details for MFA onboarding
            var secret = _totpService.GenerateSecretKey();
            var provisioningUri = _totpService.GenerateProvisioningUri(user.Employee.Email, secret);
            var qrCodeSvg = _totpService.GenerateQrCodeSvg(provisioningUri);

            return new LoginResponseDto
            {
                RequiresMfa = true,
                IsMfaSetupRequired = true,
                MfaToken = _jwtTokenService.GenerateMfaToken(user),
                QrCodeSvg = qrCodeSvg,
                Secret = secret
            };
        }

        public async Task<TokenResult> RefreshTokenAsync(
            RefreshTokenRequestDto request,
            string? userAgent,
            string? timeZone,
            CancellationToken ct = default)
        {
            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal is null)
            {
                throw new UnauthorizedException(Localization.L("InvalidToken"));
            }

            var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) ?? principal.FindFirst("sub");
            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedException(Localization.L("InvalidToken"));
            }

            var session = await _sessionRepository
                        .AsQueryable()
                        .Include(x => x.User)
                        .ThenInclude(x => x.Employee)
                        .ThenInclude(x => x.JobTitle)
                        .Where(x => x.AccessToken == request.AccessToken && x.RefreshToken == request.RefreshToken && x.UserId == userId && x.IsActive)
                        .FirstOrDefaultAsync(ct);

            if (session is null || session.RefreshTokenExpiryTime <= DateTime.UtcNow || !session.User.IsActive)
            {
                throw new UnauthorizedException(Localization.L("InvalidToken"));
            }

            var tokenResult = _jwtTokenService.GenerateToken(session.User, session.RefreshToken);

            // Update existing session with new Access Token
            session.AccessToken = tokenResult.AccessToken;
            session.AccessTokenExpiryTime = tokenResult.Expiration;
            if (userAgent != null) session.UserAgent = userAgent;
            if (timeZone != null) session.TimeZone = timeZone;

            await SaveUserSessionAsync(session, false, ct);

            return tokenResult;
        }

        public async Task<Result> LogoutAsync(
            LogoutRequestDto request,
            CancellationToken ct = default)
        {
            var userId = UserContext.UserId;
            if (userId is null)
            {
                _logger.LogWarning("Logout attempted with missing user ID in context.");
                throw new UnauthorizedException(Localization.L("InvalidToken"));
            }

            var session = await _sessionRepository
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.RefreshToken == request.RefreshToken && x.UserId == userId && x.IsActive, ct);

            if (session is null)
            {
                _logger.LogWarning("Invalid logout attempt: no active session found for User {UserId} with the provided Refresh Token.", userId);
                return Result.Failure(Localization.L("InvalidSession"));
            }

            session.IsActive = false;
            session.AccessTokenExpiryTime = DateTime.UtcNow;
            session.RefreshTokenExpiryTime = DateTime.UtcNow;
            _sessionRepository.Update(session);
            await UnitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("User {UserId} successfully logged out and session with Refresh Token revoked.", userId);

            return Result.Success(Localization.L("LoggedOutSuccessfully"));
        }

        private async Task SaveUserSessionAsync(UserSession session, bool isNew, CancellationToken ct)
        {
            if (isNew)
            {
                await _sessionRepository.AddAsync(session, ct);
            }
            else
            {
                _sessionRepository.Update(session);
            }
            await UnitOfWork.SaveChangesAsync(ct);
        }

        public async Task<LoginResponseDto> VerifyMfaAsync(
            MfaVerifyRequestDto request,
            string? userAgent,
            string? timeZone,
            CancellationToken ct = default)
        {
            var principal = _jwtTokenService.ValidateMfaToken(request.MfaToken);
            if (principal is null)
            {
                _logger.LogWarning("MFA validation failed: Invalid or expired MFA token.");
                throw new UnauthorizedException(Localization.L("InvalidMfaToken"));
            }

            var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) ?? principal.FindFirst("sub");
            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                _logger.LogWarning("MFA validation failed: Subject claim missing from token.");
                throw new UnauthorizedException(Localization.L("InvalidMfaToken"));
            }

            var user = await _userRepository
                        .AsQueryable()
                        .Include(x => x.Employee)
                        .ThenInclude(x => x.JobTitle)
                        .FirstOrDefaultAsync(x => x.Id == userId && x.IsActive, ct);

            if (user is null)
            {
                _logger.LogWarning("MFA validation failed: User {UserId} not found.", userId);
                throw new UnauthorizedException(Localization.L("UserNotFound"));
            }

            bool isSetupMode = string.IsNullOrEmpty(user.MfaSecret);
            string decryptedSecret;

            if (isSetupMode)
            {
                if (string.IsNullOrEmpty(request.Secret))
                {
                    _logger.LogWarning("MFA validation failed: User {UserId} has not set up MFA and no setup secret was provided.", userId);
                    throw new UnauthorizedException(Localization.L("MfaNotEnabled"));
                }
                decryptedSecret = request.Secret;
            }
            else
            {
                decryptedSecret = _totpService.DecryptSecret(user.MfaSecret!);
            }

            // Lockout check
            if (user.MfaLockedUntilUtc.HasValue && user.MfaLockedUntilUtc.Value > DateTime.UtcNow)
            {
                _logger.LogWarning("MFA verification blocked: User {UserId} is currently locked out until {LockedUntilUtc}.", userId, user.MfaLockedUntilUtc);
                throw new UnauthorizedException(Localization.L("MfaLockedOut"));
            }

            bool isValid = false;
            bool isRecoveryCodeUsed = false;

            // Check if it's a recovery code (usually formatted like xxxx-xxxx, i.e., contains a dash)
            if (request.Code.Contains("-"))
            {
                if (isSetupMode)
                {
                    // Recovery code cannot be used during initial setup
                    isValid = false;
                }
                else
                {
                    // Verify recovery code
                    var hashedInput = _totpService.HashRecoveryCode(request.Code);
                    var storedCodes = string.IsNullOrEmpty(user.MfaRecoveryCodes) 
                        ? new List<string>() 
                        : user.MfaRecoveryCodes.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (storedCodes.Contains(hashedInput))
                    {
                        isValid = true;
                        isRecoveryCodeUsed = true;
                        // Remove used recovery code
                        storedCodes.Remove(hashedInput);
                        user.MfaRecoveryCodes = string.Join(";", storedCodes);
                    }
                }
            }
            else
            {
                // Verify standard 6-digit code
                isValid = _totpService.VerifyCode(decryptedSecret, request.Code);
            }

            if (!isValid)
            {
                // Increment failed attempts
                user.MfaFailedAttempts++;
                _logger.LogWarning("MFA verification failed for user {UserId}. Failed attempts: {Attempts}/{MaxAttempts}.", 
                    user.Id, user.MfaFailedAttempts, _mfaSettings.MaxFailedAttempts);

                if (user.MfaFailedAttempts >= _mfaSettings.MaxFailedAttempts)
                {
                    user.MfaLockedUntilUtc = DateTime.UtcNow.AddMinutes(_mfaSettings.LockoutMinutes);
                    _logger.LogWarning("User {UserId} has been locked out from MFA for {LockoutMinutes} minutes.", 
                        user.Id, _mfaSettings.LockoutMinutes);
                }

                _userRepository.Update(user);
                await UnitOfWork.SaveChangesAsync(ct);

                if (isSetupMode)
                {
                    throw new AppException(Localization.L("MfaSecretMismatch"));
                }

                throw new AppException(Localization.L("InvalidMfaCode"));
            }

            // Successful verification
            if (isSetupMode)
            {
                user.MfaSecret = _totpService.EncryptSecret(request.Secret!);
                var recoveryCodes = _totpService.GenerateRecoveryCodes();
                var hashedCodes = recoveryCodes.Select(code => _totpService.HashRecoveryCode(code)).ToList();
                user.MfaRecoveryCodes = string.Join(";", hashedCodes);
            }

            user.MfaFailedAttempts = 0;
            user.MfaLockedUntilUtc = null;
            user.LastLoginDateUtc = DateTime.UtcNow;
            _userRepository.Update(user);

            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            var tokenResult = _jwtTokenService.GenerateToken(user, refreshToken);

            var session = new UserSession
            {
                UserId = user.Id,
                AccessToken = tokenResult.AccessToken,
                AccessTokenExpiryTime = tokenResult.Expiration,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
                UserAgent = userAgent,
                TimeZone = timeZone,
                IsActive = true
            };

            await SaveUserSessionAsync(session, true, ct);

            _logger.LogInformation("MFA verification succeeded for user {UserId}. Recovery code used: {UsedRecoveryCode}.", user.Id, isRecoveryCodeUsed);

            return new LoginResponseDto
            {
                AccessToken = tokenResult.AccessToken,
                RefreshToken = tokenResult.RefreshToken,
                RequiresMfa = false
            };
        }

        public async Task<MfaSetupResponseDto> SetupMfaAsync(CancellationToken ct = default)
        {
            var userId = UserContext.UserId;
            if (userId is null)
            {
                throw new UnauthorizedException(Localization.L("InvalidToken"));
            }

            var user = await _userRepository
                        .AsQueryable()
                        .Include(x => x.Employee)
                        .FirstOrDefaultAsync(x => x.Id == userId, ct);

            if (user is null)
            {
                throw new UnauthorizedException(Localization.L("UserNotFound"));
            }

            // Generate secret key & QR Code SVG
            var secret = _totpService.GenerateSecretKey();
            var provisioningUri = _totpService.GenerateProvisioningUri(user.Employee.Email, secret);
            var qrCodeSvg = _totpService.GenerateQrCodeSvg(provisioningUri);

            // We do not save to DB yet. Verification is required.
            return new MfaSetupResponseDto
            {
                Secret = secret,
                QrCodeSvg = qrCodeSvg
            };
        }

        public async Task<MfaEnableResponseDto> EnableMfaAsync(
            MfaEnableRequestDto request,
            CancellationToken ct = default)
        {
            var userId = UserContext.UserId;
            if (userId is null)
            {
                throw new UnauthorizedException(Localization.L("InvalidToken"));
            }

            var user = await _userRepository
                        .AsQueryable()
                        .FirstOrDefaultAsync(x => x.Id == userId, ct);

            if (user is null)
            {
                throw new UnauthorizedException(Localization.L("UserNotFound"));
            }

            // Verify code against the secret provided during setup
            var isValid = _totpService.VerifyCode(request.Secret, request.Code);
            if (!isValid)
            {
                _logger.LogWarning("MFA activation verification failed for user {UserId}.", user.Id);
                throw new AppException(Localization.L("InvalidMfaCode"));
            }

            // Encrypt and save secret
            user.MfaSecret = _totpService.EncryptSecret(request.Secret);
            user.MfaFailedAttempts = 0;
            user.MfaLockedUntilUtc = null;

            // Generate and hash recovery codes
            var recoveryCodes = _totpService.GenerateRecoveryCodes();
            var hashedCodes = new List<string>();
            foreach (var code in recoveryCodes)
            {
                hashedCodes.Add(_totpService.HashRecoveryCode(code));
            }
            user.MfaRecoveryCodes = string.Join(";", hashedCodes);

            _userRepository.Update(user);
            await UnitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("MFA enabled successfully for user {UserId}.", user.Id);

            return new MfaEnableResponseDto
            {
                RecoveryCodes = recoveryCodes
            };
        }

        public async Task<Result> DisableMfaAsync(
            MfaDisableRequestDto request,
            CancellationToken ct = default)
        {
            var userId = UserContext.UserId;
            if (userId is null)
            {
                throw new UnauthorizedException(Localization.L("InvalidToken"));
            }

            var user = await _userRepository
                        .AsQueryable()
                        .FirstOrDefaultAsync(x => x.Id == userId, ct);

            if (user is null || string.IsNullOrEmpty(user.MfaSecret))
            {
                throw new AppException(Localization.L("MfaNotEnabled"));
            }

            // Verify code against the active secret
            string decryptedSecret = _totpService.DecryptSecret(user.MfaSecret);
            var isValid = _totpService.VerifyCode(decryptedSecret, request.Code);
            if (!isValid)
            {
                _logger.LogWarning("MFA disable verification failed for user {UserId}.", user.Id);
                return Result.Failure(Localization.L("InvalidMfaCode"));
            }

            // Reset MFA properties
            user.MfaSecret = null;
            user.MfaRecoveryCodes = null;
            user.MfaFailedAttempts = 0;
            user.MfaLockedUntilUtc = null;

            _userRepository.Update(user);
            await UnitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("MFA disabled successfully for user {UserId}.", user.Id);

            return Result.Success(Localization.L("MfaDisabledSuccessfully"));
        }
    }
}
