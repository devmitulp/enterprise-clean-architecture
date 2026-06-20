using API.Extensions;
using Newtonsoft.Json;

namespace API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(
        HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    exception.Message);

                await HandleExceptionAsync(
                    context,
                    exception);
            }
        }

        private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
        {
            var response = ExceptionMapper.Map(exception, context);
            
            context.Response.StatusCode =
                response.StatusCode;

            context.Response.ContentType = "application/problem+json";

            //await context.Response.WriteAsJsonAsync(response);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
    }
}
