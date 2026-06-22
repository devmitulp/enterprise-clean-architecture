using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Auth
{
    public class CustomJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        public override ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            // Delegate the validation to the base JwtSecurityTokenHandler validation logic
            return base.ValidateToken(securityToken, validationParameters, out validatedToken);
        }

        public override Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
        {
            // Delegate the validation to the base JwtSecurityTokenHandler async validation logic
            return base.ValidateTokenAsync(token, validationParameters);
        }
    }
}
