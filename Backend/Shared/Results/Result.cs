namespace Shared.Results
{
    public class Result
    {
        public bool Succeeded { get; protected set; }

        public string Message { get; protected set; } = string.Empty;

        public string? ErrorCode { get; protected set; }

        protected Result()
        {
        }

        protected Result(
            bool succeeded,
            string message,
            string? errorCode = null)
        {
            Succeeded = succeeded;
            Message = message;
            ErrorCode = errorCode;
        }

        public static Result Success(
            string message = "")
        {
            return new Result(
                true,
                message);
        }

        public static Result Failure(
            string message,
            string? errorCode = null)
        {
            return new Result(
                false,
                message,
                errorCode);
        }
    }

    public class Result<T> : Result
    {
        public T? Data { get; private set; }

        private Result(
            bool succeeded,
            T? data,
            string message,
            string? errorCode = null)
            : base(
                succeeded,
                message,
                errorCode)
        {
            Data = data;
        }

        public static Result<T> Success(
            T data,
            string message = "")
        {
            return new Result<T>(
                true,
                data,
                message);
        }

        public static Result<T> Failure(
            string message,
            string? errorCode = null)
        {
            return new Result<T>(
                false,
                default,
                message,
                errorCode);
        }
    }
}
