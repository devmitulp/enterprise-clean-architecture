using Application.Common.Helpers;
using Application.Common.Interfaces.JwtToken;
using Application.Common.Interfaces.Localization;
using Application.Common.Interfaces.Auth;
using Application.Features.Auth;
using Application.Features.JobTitles;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Common.Helpers;
using Infrastructure.Services.Common.JwtToken;
using Infrastructure.Services.Common.Localization;
using Infrastructure.Services.JobTitles;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
        {
            // Localization Service
            services.AddSingleton<ILocalizationService, LocalizationService>();

            // JWT Token Service
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            // Password Helper Service
            services.AddScoped<IPasswordHelper, PasswordHelper>();

            // Auth Service
            services.AddScoped<IAuthService, AuthService>();

            // User Context Service
            services.AddScoped<ICurrentUserContext, CurrentUserContext>();

            // Job Title Application Service
            services.AddScoped<IJobTitleAppService, JobTitleAppService>();

            return services;
        }
    }
}
