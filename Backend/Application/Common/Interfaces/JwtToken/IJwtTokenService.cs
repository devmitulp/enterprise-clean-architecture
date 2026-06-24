using System.Security.Claims;
using Application.Common.Models;
using Domain.Entities.Users;

namespace Application.Common.Interfaces.JwtToken
{
    public interface IJwtTokenService
    {
        TokenResult GenerateToken(User user, string refreshToken);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
