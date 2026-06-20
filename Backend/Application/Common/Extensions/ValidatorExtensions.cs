using Application.Common.Interfaces.Localization;
using FluentValidation;

namespace Application.Common.Extensions
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TProperty>
        Required<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            string propertyName,
            ILocalizationService localizer)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(
                    localizer.L(
                        "Required",
                        propertyName));
        }

        public static IRuleBuilderOptions<T, string>
            MaxLengthValidation<T>(
                this IRuleBuilder<T, string> ruleBuilder,
                string propertyName,
                int length,
                ILocalizationService localizer)
        {
            return ruleBuilder
                .MaximumLength(length)
                .WithMessage(
                    localizer.L(
                        "MaxLength",
                        propertyName,
                        length));
        }
    }
}
