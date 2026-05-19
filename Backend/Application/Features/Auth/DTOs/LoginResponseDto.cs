namespace Application.Features.Auth.DTOs
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public DateTime ExpirationUtc { get; set; }
    }
}
