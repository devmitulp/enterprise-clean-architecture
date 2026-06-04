using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace API.Extensions
{
    public static class ApiConfigurationExtensions
    {
        public static IServiceCollection AddApiConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    var origins = configuration
                        .GetSection("CorsSettings:AllowedOrigins")
                        .Get<string[]>();

                    policy
                        .WithOrigins(origins ?? [])
                        .WithMethods(
                            "GET",
                            "POST",
                            "PUT",
                            "DELETE",
                            "PATCH")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders(
                            "Content-Disposition",
                            "Content-Length")
                        .SetPreflightMaxAge(TimeSpan.FromHours(24));
                });
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                [
                    "application/json",
                    "application/problem+json",
                    "application/pdf",
                    "text/csv",
                    "application/xml",
                    "text/xml",
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                ]);
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                });

            return services;
        }
    }
}
