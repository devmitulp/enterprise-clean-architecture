namespace Shared.Results
{
    public class ErrorResponse
    {
        public int StatusCode { get; init; }

        public bool Succeeded { get; init; }

        public string Message { get; init; } = string.Empty;

        public string? ErrorCode { get; init; }

        public object? Errors { get; init; }

        public string? TraceId { get; init; }
    }
}
