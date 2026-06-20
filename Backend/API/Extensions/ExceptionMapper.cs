using Shared.Constants;
using Shared.Exceptions;
using Shared.Results;

namespace API.Extensions
{
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
