using Domain.Common;
using Domain.Entities.Employees;
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

        // Navigation Property
        public Employee Employee { get; set; } = default!;
    }
}
