using System.Security.Claims;
using Application.Common.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Infrastructure.Services.Auth
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        public override Task TokenValidated(TokenValidatedContext context)
        {
            var principal = context.Principal;
            if (principal != null)
            {
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier) ?? principal.FindFirst("sub");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                {
                    UserContext.UserId = userId;
                }

                var userNameClaim = principal.FindFirst(ClaimTypes.Name) ?? principal.FindFirst("unique_name");
                if (userNameClaim != null)
                {
                    UserContext.UserName = userNameClaim.Value;
                }
            }

            return base.TokenValidated(context);
        }
    }
}
