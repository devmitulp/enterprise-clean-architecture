using Shared.Models;

namespace Application.Features.Roles.DTOs
{
    public class GetAllRolesInput : FilterRequestDto
    {
        public string? Filter { get; set; }

        public int? RoleType { get; set; }
    }
}
