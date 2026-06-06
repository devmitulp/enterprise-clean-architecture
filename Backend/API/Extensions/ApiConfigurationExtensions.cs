using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Shared.Exceptions;
using System.IO.Compression;

namespace API.Extensions
{
    public static class ApiConfigurationExtensions
    {
        public static IServiceCollection AddApiConfiguration(
     this IServiceCollection services,
     IConfiguration configuration)
        {
            // CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    var origins = configuration
                        .GetSection("CorsSettings:AllowedOrigins")
                        .Get<string[]>();

                    policy
                        .WithOrigins(origins ?? [])
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders(
                            "Content-Disposition",
                            "Content-Length")
                        .SetPreflightMaxAge(
                            TimeSpan.FromHours(24));
                });
            });

            // Compression
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;

                options.Providers.Add<GzipCompressionProvider>();

                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Concat(
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

            // MVC + Json + Validation
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = false;

                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Value!.Errors
                                .Select(e => e.ErrorMessage)
                                .ToArray());

                    throw new ValidationException(errors);
                };
            });

            return services;
        }
    }
}
