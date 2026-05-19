using Domain.Entities.Users;

namespace Application.Common.Interfaces.JwtToken
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
