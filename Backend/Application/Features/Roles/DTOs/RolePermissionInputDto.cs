namespace Application.Features.Roles.DTOs
{
    public class RolePermissionInputDto
    {
        public int? Id { get; set; }
        public int RoleId { get; set; }
        public int ApplicationMenuId { get; set; }
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool CanReview { get; set; }
    }
}
