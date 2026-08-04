using Domain.Common;
using Domain.Enums;

namespace Domain.Entities.Roles
{
    public class Role : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public RoleType RoleType { get; set; } = RoleType.Organization;

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
