using Application.Common.Helpers;
using FluentValidation;

namespace Application.Common.Extensions
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TProperty>
        Required<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            string propertyName)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(
                    LocalizationHelper.L(
                        "Required",
                        propertyName));
        }

        public static IRuleBuilderOptions<T, string>
            MaxLengthValidation<T>(
                this IRuleBuilder<T, string> ruleBuilder,
                string propertyName,
                int length)
        {
            return ruleBuilder
                .MaximumLength(length)
                .WithMessage(
                    LocalizationHelper.L(
                        "MaxLength",
                        propertyName,
                        length));
        }
    }
}
