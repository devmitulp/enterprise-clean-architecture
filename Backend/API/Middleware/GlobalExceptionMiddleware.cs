using Newtonsoft.Json;
using Shared.Constants;
using Shared.Exceptions;
using Shared.Results;

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

    internal static class ExceptionMapper
    {
        public static ErrorResponse Map(
        Exception exception, HttpContext context)
        {
            var (statusCode, errorCode, errors, message) = exception switch
            {
                ValidationException e => (StatusCodes.Status400BadRequest, e.ErrorCode, e.Errors, e.Message),
                UnauthorizedException e => (StatusCodes.Status401Unauthorized, e.ErrorCode, null, e.Message),
                ForbiddenException e => (StatusCodes.Status403Forbidden, e.ErrorCode, null, e.Message),
                NotFoundException e => (StatusCodes.Status404NotFound, e.ErrorCode, null, e.Message),
                AppException e => (StatusCodes.Status400BadRequest, e.ErrorCode, null, e.Message),
                _ => (StatusCodes.Status500InternalServerError, ErrorCodes.InternalServerError, null, "An unexpected error occurred.")
            };
            return new ErrorResponse
            {
                Succeeded = false,
                StatusCode = statusCode,
                ErrorCode = errorCode,
                Errors = errors,
                Message = message,
                TraceId = context.TraceIdentifier
            };
        }
    }

}
