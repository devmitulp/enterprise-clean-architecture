using Shared.Constants;

namespace Shared.Exceptions
{
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message= "You do not have permission to perform this action.")
        : base(message ,ErrorCodes.Forbidden)
        {
        }

    }
}
