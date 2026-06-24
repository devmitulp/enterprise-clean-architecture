using Application.Common.Extensions;
using Application.Common.Interfaces.Localization;
using Application.Common.Validators;
using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Validators
{
    public class RefreshTokenRequestValidator : BaseValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestValidator(ILocalizationService localizer)
            : base(localizer)
        {
            RuleFor(x => x.AccessToken)
                .Required(nameof(RefreshTokenRequestDto.AccessToken), L);

            RuleFor(x => x.RefreshToken)
                .Required(nameof(RefreshTokenRequestDto.RefreshToken), L);
        }
    }
}
