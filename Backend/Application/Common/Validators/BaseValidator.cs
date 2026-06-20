using Application.Common.Extensions;
using Application.Common.Interfaces.Localization;
using FluentValidation;

namespace Application.Common.Validators
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        protected readonly ILocalizationService L;

        protected BaseValidator(ILocalizationService localizer)
        {
            L = localizer;
            ValidatorExtensions.Configure(localizer);
        }
    }
}
