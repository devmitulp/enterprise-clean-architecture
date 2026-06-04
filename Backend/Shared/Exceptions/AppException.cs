using Shared.Constants;

namespace Shared.Exceptions
{
    public class AppException : Exception
    {
        public string ErrorCode { get; }
        public AppException(string message, string errorCode= ErrorCodes.AppError)
        : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
