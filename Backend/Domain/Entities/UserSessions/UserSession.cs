using Domain.Common;
using Domain.Entities.Users;

namespace Domain.Entities.UserSessions
{
    public class UserSession : BaseEntity
    {
        public int UserId { get; set; }

        public string AccessToken { get; set; } = string.Empty;

        public DateTime AccessTokenExpiryTime { get; set; }

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpiryTime { get; set; }

        public string? UserAgent { get; set; }

        public string? TimeZone { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public User User { get; set; } = default!;
    }
}
