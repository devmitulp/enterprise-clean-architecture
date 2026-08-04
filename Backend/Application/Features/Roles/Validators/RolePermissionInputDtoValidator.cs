using Application.Common.Extensions;
using Application.Common.Interfaces.Localization;
using Application.Common.Validators;
using Application.Features.Roles.DTOs;

namespace Application.Features.Roles.Validators
{
    public class RolePermissionInputDtoValidator : BaseValidator<RolePermissionInputDto>
    {
        public RolePermissionInputDtoValidator(ILocalizationService localizer)
            : base(localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThanValidation(nameof(RolePermissionInputDto.Id), 0, L, x => x.Id.HasValue);

            RuleFor(x => x.RoleId)
                .GreaterThanValidation(nameof(RolePermissionInputDto.RoleId), 0, L);

            RuleFor(x => x.ApplicationMenuId)
                .GreaterThanValidation(nameof(RolePermissionInputDto.ApplicationMenuId), 0, L);
        }
    }
}
