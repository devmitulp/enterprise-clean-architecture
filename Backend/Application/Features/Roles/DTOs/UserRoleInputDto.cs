namespace Application.Features.Roles.DTOs
{
    public class UserRoleInputDto
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
