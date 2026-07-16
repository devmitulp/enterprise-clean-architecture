using Application.Common.Extensions;
using Application.Common.Interfaces.Localization;
using Application.Common.Validators;
using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Validators
{
    public class LogoutRequestValidator : BaseValidator<LogoutRequestDto>
    {
        public LogoutRequestValidator(ILocalizationService localizer)
            : base(localizer)
        {
            RuleFor(x => x.RefreshToken)
                .Required(nameof(LogoutRequestDto.RefreshToken), L);
        }
    }
}
