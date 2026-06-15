namespace Shared.Models
{
    public class ValidationError
    {
        public string PropertyName { get; init; } = string.Empty;

        public string ErrorMessage { get; init; } = string.Empty;

        public ValidationError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }
    }
}
