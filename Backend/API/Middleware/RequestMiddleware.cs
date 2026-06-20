namespace API.Middleware
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<RequestMiddleware> _logger;

        public RequestMiddleware(
            RequestDelegate next,
            ILogger<RequestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context)
        {
            _logger.LogInformation(
                "Request: {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await _next(context);
        }
    }
}
