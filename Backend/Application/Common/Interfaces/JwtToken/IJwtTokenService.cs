using Application.Common.Models;
using Domain.Entities.Users;

namespace Application.Common.Interfaces.JwtToken
{
    public interface IJwtTokenService
    {
        TokenResult GenerateToken(User user);
    }
}
