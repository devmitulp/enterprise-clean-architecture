using Application.Common.Models;
using Application.Features.Auth.DTOs;

namespace Application.Features.Auth
{
    public interface IAuthService
    {
        Task<TokenResult> LoginAsync(LoginRequestDto request, string? userAgent, string? timeZone, CancellationToken ct = default);
        Task<TokenResult> RefreshTokenAsync(RefreshTokenRequestDto request, string? userAgent, string? timeZone, CancellationToken ct = default);
    }
}
