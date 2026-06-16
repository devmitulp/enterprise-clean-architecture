namespace Application.Common.Models
{
    public class TokenResult
    {
        public string AccessToken { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }

        public int ExpiresInMinutes { get; set; }
    }
}
