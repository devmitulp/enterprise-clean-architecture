using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Shared.Constants;
using Shared.Models;
using Shared.Results;
using System.IO.Compression;

namespace API.Extensions
{
    public static class ApiConfigurationExtensions
    {
        /// <summary>
        /// Configures forwarded headers so that the application correctly reads
        /// the client IP and protocol when running behind a reverse proxy (e.g. nginx, YARP, Azure App Gateway).
        /// </summary>
        public static IServiceCollection AddForwardedHeadersConfiguration(
            this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
                    Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;

                // Trust all proxies/networks — narrow this down in production
                // to the actual known proxy IP ranges.
                options.KnownIPNetworks.Clear();
                options.KnownProxies.Clear();
            });

            return services;
        }

        /// <summary>
        /// Configures the CORS default policy from <c>CorsSettings:AllowedOrigins</c>
        /// in configuration.
        /// </summary>
        public static IServiceCollection AddCorsPolicy(
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

            return services;
        }

        /// <summary>
        /// Registers GZIP response compression for JSON, PDF, CSV, XML, and
        /// Office Open XML MIME types.
        /// </summary>
        public static IServiceCollection AddGzipResponseCompression(
            this IServiceCollection services)
        {
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
                options.Level = CompressionLevel.Fastest);

            return services;
        }

        /// <summary>
        /// Registers MVC controllers with Pascal-case JSON serialization and a
        /// custom model-validation error response that maps validation failures
        /// to the application's standard <see cref="ErrorResponse"/> shape.
        /// </summary>
        public static IServiceCollection AddMvcConfiguration(
            this IServiceCollection services)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    // Preserve Pascal-case property names — matches the DTOs as written.
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = false;

                // Return a structured ErrorResponse instead of the default ProblemDetails
                // so that clients always receive a consistent error envelope.
                options.InvalidModelStateResponseFactory = context =>
                {
                    // Determine field order from the DTO so errors are returned
                    // in the same top-to-bottom order they appear on the request type.
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

                    var response = new ErrorResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Succeeded = false,
                        Message = "One or more validation errors occurred.",
                        ErrorCode = ErrorCodes.ValidationError,
                        Errors = errors,
                        TraceId = context.HttpContext.TraceIdentifier
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }
    }
}
