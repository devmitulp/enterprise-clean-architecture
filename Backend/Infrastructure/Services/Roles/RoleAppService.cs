using System.Linq.Dynamic.Core;
using Application.Common.Interfaces.Base;
using Application.Common.Interfaces.Persistence;
using Application.Features.Roles;
using Application.Features.Roles.DTOs;
using Domain.Entities.Roles;
using Domain.Enums;
using Infrastructure.Services.Common.Base;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using Shared.Models;

namespace Infrastructure.Services.Roles
{
    public class RoleAppService : ApplicationBaseService, IRoleAppService
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole> _userRoleRepository;

        public RoleAppService(
            IServiceContext context,
            IRepository<Role> roleRepository,
            IRepository<UserRole> userRoleRepository) : base(context)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        #region Public Methods

        public async Task<PagedResultDto<RoleDto>> GetAllAsync(GetAllRolesInput input, CancellationToken ct = default)
        {
            bool isProductAdmin = await IsCurrentProductAdminAsync(ct);

            IQueryable<Role> query = _roleRepository.AsQueryable();

            if (!isProductAdmin)
            {
                query = query.Where(x => x.RoleType != RoleType.ProductAdmin);
            }

            if (input.RoleType.HasValue)
            {
                query = query.Where(x => (int)x.RoleType == input.RoleType.Value);
            }

            query = query
                .Where(x => string.IsNullOrWhiteSpace(input.Filter) ||
                            x.Name.Contains(input.Filter) ||
                            (x.Description != null && x.Description.Contains(input.Filter)))
                .OrderBy(string.IsNullOrWhiteSpace(input.Sorting) ? "Name ASC" : input.Sorting);

            var totalCount = await query.CountAsync(ct);

            if (input.ApplyPagination == true)
            {
                int page = Math.Max(input.CurrentPage ?? 1, 1);
                int size = input.PageSize is > 0 ? input.PageSize.Value : 10;
                query = query.Skip((page - 1) * size).Take(size);
            }

            return new PagedResultDto<RoleDto>(
                totalCount,
                ObjectMapper.Map<IReadOnlyList<RoleDto>>(await query.ToListAsync(ct)));
        }

        public async Task<RoleDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _roleRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException(Localization.L("EntityNotFound", "Role", id));

            bool isProductAdmin = await IsCurrentProductAdminAsync(ct);
            if (!isProductAdmin && entity.RoleType == RoleType.ProductAdmin)
            {
                throw new NotFoundException(Localization.L("EntityNotFound", "Role", id));
            }

            return ObjectMapper.Map<RoleDto>(entity);
        }

        public async Task<RoleDto> CreateOrUpdateRoleAsync(RoleInputDto input, CancellationToken ct = default)
        {
            if (input.Id.HasValue && input.Id.Value > 0)
            {
                return await Update(input, ct);
            }
            else
            {
                return await Create(input, ct);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _roleRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException(Localization.L("EntityNotFound", "Role", id));

            bool isProductAdmin = await IsCurrentProductAdminAsync(ct);
            if (!isProductAdmin && entity.RoleType == RoleType.ProductAdmin)
            {
                throw new ForbiddenException(Localization.L("CannotManageProductAdminRole"));
            }

            _roleRepository.Remove(entity);
            await UnitOfWork.SaveChangesAsync(ct);
        }

        #endregion

        #region Private Methods

        private async Task<bool> IsCurrentProductAdminAsync(CancellationToken ct = default)
        {
            if (!UserContext.UserId.HasValue)
            {
                return false;
            }

            return await _userRoleRepository.AsQueryable()
                .AnyAsync(ur => ur.UserId == UserContext.UserId.Value && ur.Role.RoleType == RoleType.ProductAdmin, ct);
        }

        private async Task CheckRoleNameExistAsync(string name, int? excludeId = null, CancellationToken ct = default)
        {
            var nameExists = await _roleRepository.ExistsAsync(
                x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase) && (!excludeId.HasValue || x.Id != excludeId.Value), ct);

            if (nameExists)
                throw new AppException(Localization.L("RoleAlreadyExists", name));
        }

        private async Task<RoleDto> Create(RoleInputDto input, CancellationToken ct = default)
        {
            bool isProductAdmin = await IsCurrentProductAdminAsync(ct);
            if (!isProductAdmin && (RoleType)input.RoleType == RoleType.ProductAdmin)
            {
                throw new ForbiddenException(Localization.L("CannotManageProductAdminRole"));
            }

            await CheckRoleNameExistAsync(input.Name, ct: ct);

            var entity = ObjectMapper.Map<Role>(input);
            await _roleRepository.AddAsync(entity, ct);
            await UnitOfWork.SaveChangesAsync(ct);

            return ObjectMapper.Map<RoleDto>(entity);
        }

        private async Task<RoleDto> Update(RoleInputDto input, CancellationToken ct = default)
        {
            var id = input.Id ?? throw new AppException(Localization.L("RoleIdRequiredForUpdate"));
            var entity = await _roleRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException(Localization.L("EntityNotFound", "Role", id));

            bool isProductAdmin = await IsCurrentProductAdminAsync(ct);
            if (!isProductAdmin && (entity.RoleType == RoleType.ProductAdmin || (RoleType)input.RoleType == RoleType.ProductAdmin))
            {
                throw new ForbiddenException(Localization.L("CannotManageProductAdminRole"));
            }

            await CheckRoleNameExistAsync(input.Name, id, ct);

            ObjectMapper.Map(input, entity);
            _roleRepository.Update(entity);
            await UnitOfWork.SaveChangesAsync(ct);

            return ObjectMapper.Map<RoleDto>(entity);
        }

        #endregion
    }
}
