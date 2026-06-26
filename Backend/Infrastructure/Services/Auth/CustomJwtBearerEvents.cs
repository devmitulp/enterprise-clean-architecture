using Application.Common.Interfaces.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Infrastructure.Services.Auth
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<CustomJwtBearerEvents> _logger;

        public CustomJwtBearerEvents(IUserContext userContext, ILogger<CustomJwtBearerEvents> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        public override Task TokenValidated(TokenValidatedContext context)
        {
            var principal = context.Principal;
            if (principal != null)
            {
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier) ?? principal.FindFirst("sub");
                var userNameClaim = principal.FindFirst(ClaimTypes.Name) ?? principal.FindFirst("unique_name");

                if (_userContext is UserContext userContextConcrete)
                {
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                    {
                        userContextConcrete.UserId = userId;
                    }

                    if (userNameClaim != null)
                    {
                        userContextConcrete.UserName = userNameClaim.Value;
                    }
                }
            }

            return base.TokenValidated(context);
        }
        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            _logger.LogError(context.Exception, "Authentication failed for request at {Path}", context.HttpContext.Request.Path);
            return base.AuthenticationFailed(context);
        }
    }
}
