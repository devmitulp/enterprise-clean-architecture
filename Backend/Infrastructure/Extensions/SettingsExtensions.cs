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
            services.Configure<JwtSettings>(
                configuration.GetSection("JwtSettings"));

            return services;
        }
    }
}
