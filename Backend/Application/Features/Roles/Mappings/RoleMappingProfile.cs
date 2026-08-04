using AutoMapper;
using Domain.Entities.Roles;
using Application.Features.Roles.DTOs;
using Domain.Enums;

namespace Application.Features.Roles.Mappings
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.RoleType, opt => opt.MapFrom(src => (int)src.RoleType));

            CreateMap<RoleInputDto, Role>()
                .ForMember(dest => dest.RoleType, opt => opt.MapFrom(src => (RoleType)src.RoleType))
                .ForMember(dest => dest.CreatedDateUtc, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDateUtc, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id.HasValue));
                
            CreateMap<UserRole, UserRoleDto>();
            
            CreateMap<UserRoleInputDto, UserRole>()
                .ForMember(dest => dest.CreatedDateUtc, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDateUtc, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id.HasValue));

            CreateMap<RolePermission, RolePermissionDto>();
            
            CreateMap<RolePermissionInputDto, RolePermission>()
                .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id.HasValue));
        }
    }
}
