using Domain.Common;
using Domain.Entities.Employees;
using Domain.Entities.UserSessions;
using Domain.Enums;

namespace Domain.Entities.Users
{
    public class User : AuditableEntity
    {
        public int EmployeeId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public DateTime? LastLoginDateUtc { get; set; }

        public LoginProvider LoginProvider { get; set; }

        public string? ExternalProviderId { get; set; }

        public string? MfaSecret { get; set; }

        public int MfaFailedAttempts { get; set; }

        public DateTime? MfaLockedUntilUtc { get; set; }

        public string? MfaRecoveryCodes { get; set; } // Semicolon-delimited list of hashed recovery codes

        // Navigation Property
        public Employee Employee { get; set; } = default!;

        public ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
    }
}
