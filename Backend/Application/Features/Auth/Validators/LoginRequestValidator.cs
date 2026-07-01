using Application.Common.Extensions;
using Application.Common.Interfaces.Localization;
using Application.Common.Validators;
using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Validators
{
    public class LoginRequestValidator : BaseValidator<LoginRequestDto>
    {
        public LoginRequestValidator(ILocalizationService localizer)
            : base(localizer)
        {
            RuleFor(x => x.Email)
                .Required(nameof(LoginRequestDto.Email), L)
                .MaxLengthValidation(nameof(LoginRequestDto.Email), 100, L);

            RuleFor(x => x.Password)
                .Required(nameof(LoginRequestDto.Password), L);
        }
    }
}
