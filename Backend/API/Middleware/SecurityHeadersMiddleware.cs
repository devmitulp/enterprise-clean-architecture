using System.Reflection.PortableExecutable;

namespace API.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
        HttpContext context)
        {
            var headers = context.Response.Headers;

            // X-Content-Type-Options
            headers.TryAdd(
                "X-Content-Type-Options",
                "nosniff");

            // Referrer Policy
            headers.TryAdd(
                "Referrer-Policy",
                "strict-origin-when-cross-origin");

            // Prevent Clickjacking
            headers.TryAdd(
                "X-Frame-Options",
                "DENY");

            // Content Security Policy
            headers.TryAdd(
                "Content-Security-Policy",
                "default-src 'none'; " +
                "frame-ancestors 'none'; " +
                "base-uri 'none'; " +
                "form-action 'none';");

            // Permissions Policy
            headers.TryAdd(
                "Permissions-Policy",
                "camera=(), microphone=(), geolocation=()");

            headers.TryAdd("Cross-Origin-Resource-Policy", "same-origin");

            // HTTP Strict Transport Security (HTTPS only)
            if (context.Request.IsHttps)
            {
                headers.TryAdd(
                    "Strict-Transport-Security",
                    "max-age=31536000; includeSubDomains");
            }

            // Continue request pipeline
            await _next(context);
        }
    }
}
