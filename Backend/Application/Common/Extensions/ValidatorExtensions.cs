using Application.Common.Interfaces.Localization;
using FluentValidation;
using System;

namespace Application.Common.Extensions
{
    public static class ValidatorExtensions
    {
        private static ILocalizationService? _localizer;

        public static void Configure(ILocalizationService localizer)
        {
            if (_localizer != null)
            {
                return; // Guard to keep the reference write-protected (read-only)
            }
            _localizer = localizer;
        }

        private static ILocalizationService Localizer => _localizer ?? throw new InvalidOperationException("Localization service is not configured.");

        public static IRuleBuilderOptions<T, TProperty>
        Required<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            string propertyName)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(
                    Localizer.L(
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
                    Localizer.L(
                        "MaxLength",
                        propertyName,
                        length));
        }
    }
}
