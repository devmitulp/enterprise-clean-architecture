using Application.Common.Interfaces.Localization;
using FluentValidation;
using System.Collections.Generic;

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
        {
            var options = ruleBuilder
                .Must(x => x is null || Comparer<TProperty>.Default.Compare(x, valueToCompare) > 0)
                .WithMessage(
                    localizer.L(
                        "GreaterThan",
                        propertyName,
                        valueToCompare));

            return condition != null ? options.When(condition) : options;
        }
        public static IRuleBuilderOptions<T, int>
        ValidEnumValidation<T, TEnum>(
            this IRuleBuilder<T, int> ruleBuilder,
            string propertyName,
            ILocalizationService localizer,
            Func<T, bool>? condition = null)
            where TEnum : struct, Enum
        {
            var options = ruleBuilder
                .Must(x => Enum.IsDefined(typeof(TEnum), x))
                .WithMessage(localizer.L("ValidEnum", propertyName));

            return condition != null ? options.When(condition) : options;
        }

        public static IRuleBuilderOptions<T, TProperty>
        MinValidation<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            string propertyName,
            TProperty min,
            ILocalizationService localizer,
            Func<T, bool>? condition = null)
        {
            var options = ruleBuilder
                .Must(x => x is null || Comparer<TProperty>.Default.Compare(x, min) >= 0)
                .WithMessage(
                    localizer.L(
                        "Min",
                        propertyName,
                        min));

            return condition != null ? options.When(condition) : options;
        }

        public static IRuleBuilderOptions<T, TProperty>
        MaxValidation<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            string propertyName,
            TProperty max,
            ILocalizationService localizer,
            Func<T, bool>? condition = null)
        {
            var options = ruleBuilder
                .Must(x => x is null || Comparer<TProperty>.Default.Compare(x, max) <= 0)
                .WithMessage(
                    localizer.L(
                        "Max",
                        propertyName,
                        max));

            return condition != null ? options.When(condition) : options;
        }
    }
}
