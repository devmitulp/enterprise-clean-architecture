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
            ILocalizationService localizer,
            Func<T, bool>? condition = null)
        {
            var options = ruleBuilder
                .NotEmpty()
                .WithMessage(
                    localizer.L(
                        "Required",
                        propertyName));

            return condition != null ? options.When(condition) : options;
        }

        public static IRuleBuilderOptions<T, string>
            MaxLengthValidation<T>(
                this IRuleBuilder<T, string> ruleBuilder,
                string propertyName,
                int length,
                ILocalizationService localizer,
                Func<T, bool>? condition = null)
        {
            var options = ruleBuilder
                .MaximumLength(length)
                .WithMessage(
                    localizer.L(
                        "MaxLength",
                        propertyName,
                        length));

            return condition != null ? options.When(condition) : options;
        }

        public static IRuleBuilderOptions<T, TProperty>
        GreaterThanValidation<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            string propertyName,
            TProperty valueToCompare,
            ILocalizationService localizer,
            Func<T, bool>? condition = null)
            where TProperty : IComparable<TProperty>, IComparable
        {
            var options = ruleBuilder
                .GreaterThan(valueToCompare)
                .WithMessage(
                    localizer.L(
                        "GreaterThan",
                        propertyName,
                        valueToCompare));

            return condition != null ? options.When(condition) : options;
        }

        public static IRuleBuilderOptions<T, int?>
        GreaterThanValidation<T>(
            this IRuleBuilder<T, int?> ruleBuilder,
            string propertyName,
            int valueToCompare,
            ILocalizationService localizer,
            Func<T, bool>? condition = null)
        {
            var options = ruleBuilder
                .GreaterThan(valueToCompare)
                .WithMessage(
                    localizer.L(
                        "GreaterThan",
                        propertyName,
                        valueToCompare));

            return condition != null ? options.When(condition) : options;
        }
    }
}
