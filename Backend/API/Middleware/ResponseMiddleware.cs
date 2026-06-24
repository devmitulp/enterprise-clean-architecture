using System.Diagnostics;

namespace API.Middleware
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ResponseMiddleware> _logger;

        public ResponseMiddleware(
            RequestDelegate next,
            ILogger<ResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            await _next(context);

            sw.Stop();

            _logger.LogInformation(
                "Response: {StatusCode} completed in {Duration} ms",
                context.Response.StatusCode,
                sw.ElapsedMilliseconds);
        }
    }
}
