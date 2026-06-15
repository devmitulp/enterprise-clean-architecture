using Shared.Constants;
using Shared.Models;

namespace Shared.Exceptions
{
    public class ValidationException : AppException
    {
        public IReadOnlyCollection<ValidationError> Errors { get; }

        public ValidationException(string message)
            : base(message, ErrorCodes.ValidationError)
        {
            Errors = Array.Empty<ValidationError>();
        }


        public ValidationException(IReadOnlyCollection<ValidationError> errors)
            : base("One or more validation errors occurred.", ErrorCodes.ValidationError)
        {
            Errors = errors.ToList().AsReadOnly();
        }
        public ValidationException(string propertyName, string errorMessage)
            : base("One or more validation errors occurred.", ErrorCodes.ValidationError)
        {
            Errors = new List<ValidationError>
        {
            new ValidationError(propertyName, errorMessage)
        }.AsReadOnly();
        }
    }
}
