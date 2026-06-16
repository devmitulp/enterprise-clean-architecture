using Infrastructure.Settings;
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
            services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection("JwtSettings"))
                .Validate(
                    settings =>
                        !string.IsNullOrWhiteSpace(settings.SecretKey) &&
                        settings.SecretKey.Length >= 32,
                    "JwtSettings:SecretKey is required and must be at least 32 characters. " +
                    "Set it via User Secrets (Development) or environment variable JwtSettings__SecretKey (Production).")
                .Validate(
                    settings =>
                        !string.IsNullOrWhiteSpace(settings.Issuer) &&
                        !string.IsNullOrWhiteSpace(settings.Audience) &&
                        settings.ExpiryMinutes > 0,
                    "JwtSettings: Issuer, Audience, and ExpiryMinutes must be configured.")
                .ValidateOnStart();

            return services;
        }
    }
}
