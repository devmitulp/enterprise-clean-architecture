using Application.Common.Helpers;
using Application.Common.Interfaces.JwtToken;
using Application.Features.Auth;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Common.Helpers;
using Infrastructure.Services.Common.JwtToken;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
        {
            // JWT Token Service
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            // Password Helper Service
            services.AddScoped<IPasswordHelper, PasswordHelper>();

            // Auth Service
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
