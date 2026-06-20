using API.Middleware;

namespace API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApplicationMiddleware(
        this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseMiddleware<RequestMiddleware>();

            app.UseMiddleware<ResponseMiddleware>();

            app.UseMiddleware<SecurityHeadersMiddleware>();

            return app;
        }
    }
}
