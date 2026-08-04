using Application.Common.Extensions;
using Application.Common.Interfaces.Localization;
using Application.Common.Validators;
using Application.Features.Roles.DTOs;

namespace Application.Features.Roles.Validators
{
    public class UserRoleInputDtoValidator : BaseValidator<UserRoleInputDto>
    {
        public UserRoleInputDtoValidator(ILocalizationService localizer)
            : base(localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThanValidation(nameof(UserRoleInputDto.Id), 0, L, x => x.Id.HasValue);

            RuleFor(x => x.UserId)
                .GreaterThanValidation(nameof(UserRoleInputDto.UserId), 0, L);

            RuleFor(x => x.RoleId)
                .GreaterThanValidation(nameof(UserRoleInputDto.RoleId), 0, L);
        }
    }
}
