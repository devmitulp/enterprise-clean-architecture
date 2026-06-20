using Application.Common.Extensions;
using Application.Common.Interfaces.Localization;
using Application.Common.Validators;
using Application.Features.Auth.DTOs;
using FluentValidation;

namespace Application.Features.Auth.Validators
{
    public class LoginRequestValidator : BaseValidator<LoginRequestDto>
    {
        public LoginRequestValidator(ILocalizationService localizer) : base(localizer)
        {
            RuleFor(x => x.UserName)
                .Required(nameof(LoginRequestDto.UserName))
                .MaxLengthValidation(nameof(LoginRequestDto.UserName), 100);

            RuleFor(x => x.Password)
                .Required(nameof(LoginRequestDto.Password));
        }
    }
}
