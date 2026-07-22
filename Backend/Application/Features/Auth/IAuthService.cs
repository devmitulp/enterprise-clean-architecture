using Application.Common.Models;
using Application.Features.Auth.DTOs;
using Shared.Results;

namespace Application.Features.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request, string? userAgent, string? timeZone, CancellationToken ct = default);
        Task<TokenResult> RefreshTokenAsync(RefreshTokenRequestDto request, string? userAgent, string? timeZone, CancellationToken ct = default);
        Task<Result> LogoutAsync(LogoutRequestDto request, CancellationToken ct = default);
        
        Task<LoginResponseDto> VerifyMfaAsync(MfaVerifyRequestDto request, string? userAgent, string? timeZone, CancellationToken ct = default);
        Task<MfaSetupResponseDto> SetupMfaAsync(CancellationToken ct = default);
        Task<MfaEnableResponseDto> EnableMfaAsync(MfaEnableRequestDto request, CancellationToken ct = default);
        Task<Result> DisableMfaAsync(MfaDisableRequestDto request, CancellationToken ct = default);
    }
}
