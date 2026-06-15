using Shared.Constants;
using Shared.Results;
using System.Threading.RateLimiting;

namespace API.Extensions
{
    public static class RateLimitingExtensions
    {
        public static IServiceCollection AddRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode =
                    StatusCodes.Status429TooManyRequests;

                options.OnRejected = async (
                    context,
                    cancellationToken) =>
                {
                    var response =
                        Result<string>.Failure(
                            "Too many requests. Please try again later.");

                    await context.HttpContext.Response
                        .WriteAsJsonAsync(
                            response,
                            cancellationToken);
                };

                // Login API
                options.AddPolicy(
                    RateLimitPolicies.Login,
                    context =>
                    {
                        var key =
                            context.User.Identity?.IsAuthenticated == true
                                ? context.User.Identity.Name!
                                : context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                        return RateLimitPartition.GetFixedWindowLimiter(
                            partitionKey: key,
                            factory: _ => new FixedWindowRateLimiterOptions
                            {
                                PermitLimit = 5,
                                Window = TimeSpan.FromMinutes(1),
                                QueueLimit = 0,
                                AutoReplenishment = true
                            });
                    });

                // Upload API
                options.AddPolicy(
                    RateLimitPolicies.Upload,
                    context =>
                    {
                        var key =
                            context.User.Identity?.IsAuthenticated == true
                                ? context.User.Identity.Name!
                                : context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                        return RateLimitPartition.GetFixedWindowLimiter(
                            partitionKey: key,
                            factory: _ => new FixedWindowRateLimiterOptions
                            {
                                PermitLimit = 10,
                                Window = TimeSpan.FromMinutes(1),
                                QueueLimit = 0,
                                AutoReplenishment = true
                            });
                    });

                // General APIs
                options.AddPolicy(
                    RateLimitPolicies.Api,
                    context =>
                    {
                        var key =
                            context.User.Identity?.IsAuthenticated == true
                                ? context.User.Identity.Name!
                                : context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                        return RateLimitPartition.GetFixedWindowLimiter(
                            partitionKey: key,
                            factory: _ => new FixedWindowRateLimiterOptions
                            {
                                PermitLimit = 100,
                                Window = TimeSpan.FromMinutes(1),
                                QueueLimit = 0,
                                AutoReplenishment = true
                            });
                    });
            });

            return services;
        }
    }
}
