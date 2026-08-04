using Shared.Models;

namespace Application.Features.Roles.DTOs
{
    public class UserRoleDto : AuditableEntityDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
