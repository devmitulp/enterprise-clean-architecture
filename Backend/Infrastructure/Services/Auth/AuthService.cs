using Application.Common.Helpers;
using Application.Common.Interfaces.Base;
using Application.Common.Interfaces.JwtToken;
using Application.Common.Interfaces.Persistence;
using Application.Common.Models;
using Application.Common.Settings;
using Application.Features.Auth;
using Application.Features.Auth.DTOs;
using Domain.Entities.Users;
using Domain.Entities.UserSessions;
using Infrastructure.Services.Common.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Exceptions;

namespace Infrastructure.Services.Auth
{
    public class AuthService : ApplicationBaseService, IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserSession> _sessionRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHelper _passwordHelper;

        private readonly JwtSettings _jwtSettings;

        public AuthService(
            IServiceContext context,
            IRepository<User> userRepository,
            IRepository<UserSession> sessionRepository,
            IJwtTokenService jwtTokenService,
            IPasswordHelper passwordHelper,
            IOptions<JwtSettings> jwtSettings) : base(context)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHelper = passwordHelper;

            _jwtSettings = jwtSettings.Value;
        }

        public async Task<TokenResult> LoginAsync(
            LoginRequestDto request,
            string? userAgent,
            string? timeZone,
            CancellationToken ct = default)
        {
            var user = await _userRepository
                        .AsQueryable()
                        .Include(x => x.Employee)
                        .ThenInclude(x => x.JobTitle)
                        .Where(x => x.UserName == request.UserName && x.IsActive)
                        .FirstOrDefaultAsync(ct);

            if (user is null)
                throw new UnauthorizedException(Localization.L("InvalidCredentials"));

            var isValidPassword = _passwordHelper.VerifyPassword(
                user,
                request.Password,
                user.PasswordHash);

            if (!isValidPassword)
                throw new UnauthorizedException(Localization.L("InvalidCredentials"));

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

            user.LastLoginDateUtc = DateTime.UtcNow;
            _userRepository.Update(user);

            await SaveUserSessionAsync(session, true, ct);

            return tokenResult;
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
    }
}
