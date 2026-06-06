using API.Middleware;

namespace API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApplicationMiddleware(
        this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestMiddleware>();

            app.UseMiddleware<ResponseMiddleware>();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            return app;
        }
    }
}
