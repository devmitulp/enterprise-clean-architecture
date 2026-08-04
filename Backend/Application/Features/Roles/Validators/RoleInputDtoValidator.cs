using Application.Common.Extensions;
using Application.Common.Interfaces.Localization;
using Application.Common.Validators;
using Application.Features.Roles.DTOs;
using Domain.Enums;
using FluentValidation;

namespace Application.Features.Roles.Validators
{
    public class RoleInputDtoValidator : BaseValidator<RoleInputDto>
    {
        public RoleInputDtoValidator(ILocalizationService localizer)
            : base(localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThanValidation(nameof(RoleInputDto.Id), 0, L, x => x.Id.HasValue);

            RuleFor(x => x.Name)
                .Required(nameof(RoleInputDto.Name), L)
                .MaxLengthValidation(nameof(RoleInputDto.Name), 100, L);

            RuleFor(x => x.Description!)
                .MaxLengthValidation(nameof(RoleInputDto.Description), 500, L)
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.RoleType)
                .ValidEnumValidation<RoleInputDto, RoleType>(nameof(RoleInputDto.RoleType), L);
        }
    }
}
