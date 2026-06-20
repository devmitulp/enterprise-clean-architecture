using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Shared.Exceptions;
using Shared.Models;
using System.IO.Compression;

namespace API.Extensions
{
    public static class ApiConfigurationExtensions
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            // Configure Forwarded Headers for reverse proxies
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
                options.KnownIPNetworks.Clear();
                options.KnownProxies.Clear();
            });

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
                    var dtoType = context.ActionDescriptor.Parameters
                                  .Select(p => p.ParameterType)
                                  .FirstOrDefault(t =>
                                     t != typeof(string) &&
                                     !t.IsPrimitive);

                    var propertyOrder = dtoType?
                        .GetProperties()
                        .Select((p, i) => new { p.Name, i })
                        .ToDictionary(x => x.Name, x => x.i)
                        ?? new Dictionary<string, int>();

                    var errors = context.ModelState
                                .SelectMany(modelState =>
                                    modelState.Value?.Errors.Select(error =>
                                        new ValidationError(
                                            modelState.Key,
                                            error.ErrorMessage))
                                    ?? Enumerable.Empty<ValidationError>())
                                .OrderBy(x =>
                                    propertyOrder.TryGetValue(
                                        x.PropertyName,
                                        out var index)
                                            ? index
                                            : int.MaxValue)
                                .ToList();

                    throw new ValidationException(errors);
                };
            });

            return services;
        }
    }
}
