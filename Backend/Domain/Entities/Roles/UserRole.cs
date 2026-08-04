using Domain.Common;
using Domain.Entities.Users;

namespace Domain.Entities.Roles
{
    public class UserRole : AuditableEntity
    {
        public int UserId { get; set; }
        
        public int RoleId { get; set; }

        public User User { get; set; } = default!;
        
        public Role Role { get; set; } = default!;
    }
}
