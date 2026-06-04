using Shared.Constants;

namespace Shared.Exceptions
{
    public class NotFoundException : AppException
    {

        public NotFoundException(string message)
       : base(message,ErrorCodes.NotFound)
        {
        }

        public NotFoundException(string entityName,object key)
        : base($"{entityName} with identifier '{key}' was not found.", ErrorCodes.NotFound)
        {
        }
    }
}
