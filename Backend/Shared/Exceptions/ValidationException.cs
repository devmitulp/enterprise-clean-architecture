using Shared.Constants;

namespace Shared.Exceptions
{
    public class ValidationException : AppException
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; }

        public ValidationException(string message)
            : base(message, ErrorCodes.ValidationError)
        {
            Errors = new Dictionary<string, string[]>();
        }


        public ValidationException(IDictionary<string, string[]> errors)
            : base("One or more validation errors occurred.", ErrorCodes.ValidationError)
        {
            Errors = new Dictionary<string, string[]>(errors);
        }
        public ValidationException(string propertyName,string errorMessage)
            : base("One or more validation errors occurred.", ErrorCodes.ValidationError)
        {
            Errors = new Dictionary<string, string[]>
            {
                [propertyName] = new[] { errorMessage }
            };
        }
    }
}
