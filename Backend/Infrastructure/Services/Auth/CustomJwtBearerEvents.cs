using Application.Common.Interfaces.Auth;
using Application.Common.Interfaces.Persistence;
using Domain.Entities.UserSessions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace Infrastructure.Services.Auth
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<CustomJwtBearerEvents> _logger;
        private readonly IRepository<UserSession> _sessionRepository;

        public CustomJwtBearerEvents(
            IUserContext userContext, 
            ILogger<CustomJwtBearerEvents> logger,
            IRepository<UserSession> sessionRepository)
        {
            _userContext = userContext;
            _logger = logger;
            _sessionRepository = sessionRepository;
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            var principal = context.Principal;
            if (principal != null)
            {
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier) ?? principal.FindFirst("sub");
                var userNameClaim = principal.FindFirst(ClaimTypes.Name) ?? principal.FindFirst("unique_name");

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                {
                    // Retrieve raw access token to check in database
                    var accessToken = string.Empty;
                    var authHeader = context.Request.Headers.Authorization.ToString();
                    if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        accessToken = authHeader.Substring("Bearer ".Length).Trim();
                    }

                    if (string.IsNullOrEmpty(accessToken) && context.SecurityToken != null)
                    {
                        try
                        {
                            dynamic dynToken = context.SecurityToken;
                            accessToken = dynToken.RawData ?? dynToken.EncodedToken ?? string.Empty;
                        }
                        catch
                        {
                            // Suppress dynamic binder exceptions
                            throw new InvalidOperationException("Unable to retrieve the raw access token from the security token.");
                        }
                    }

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        var isSessionActive = await _sessionRepository
                            .AsQueryable()
                            .AnyAsync(x => x.AccessToken == accessToken && x.UserId == userId && x.IsActive);

                        if (!isSessionActive)
                        {
                            _logger.LogWarning("Token validation failed: session for user {UserId} is inactive or revoked.", userId);
                            context.Fail("This session has been logged out or revoked.");
                            return;
                        }
                    }

                    if (_userContext is UserContext userContextConcrete)
                    {
                        userContextConcrete.UserId = userId;
                        if (userNameClaim != null)
                        {
                            userContextConcrete.UserName = userNameClaim.Value;
                        }
                    }
                }
            }

            await base.TokenValidated(context);
        }
        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            _logger.LogError(context.Exception, "Authentication failed for request at {Path}", context.HttpContext.Request.Path);
            return base.AuthenticationFailed(context);
        }
    }
}
