using Application.Common.Extensions;
using Application.Common.Helpers;
using Application.Features.Auth.DTOs;
using FluentValidation;

namespace Application.Features.Auth.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName)
                .Required(nameof(LoginRequestDto.UserName))
                .MaxLengthValidation(
                    nameof(LoginRequestDto.UserName),
                    100); ;

            RuleFor(x => x.Password)
                .Required(nameof(LoginRequestDto.Password));
        }
    }
}
