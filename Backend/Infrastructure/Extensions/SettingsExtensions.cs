using Application.Common.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class SettingsExtensions
    {
        public static IServiceCollection AddApplicationSettings(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.Configure<JwtSettings>(
                configuration.GetSection("JwtSettings"));

            services.Configure<PasswordHasherOptions>(
                configuration.GetSection("PasswordHasherOptions"));

            return services;
        }
    }
}
