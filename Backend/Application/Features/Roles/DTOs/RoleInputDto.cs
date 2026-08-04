namespace Application.Features.Roles.DTOs
{
    public class RoleInputDto
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int RoleType { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
