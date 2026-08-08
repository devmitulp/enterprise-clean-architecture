using Shared.Models;
using Application.Features.Roles.DTOs;

namespace Application.Features.Roles
{
    public interface IRoleAppService
    {
        Task<PagedResultDto<RoleDto>> GetAllAsync(GetAllRolesInput input, CancellationToken ct = default);

        Task<RoleDto> GetByIdAsync(int id, CancellationToken ct = default);

        Task<RoleDto> CreateOrUpdateRoleAsync(RoleInputDto input, CancellationToken ct = default);

        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
