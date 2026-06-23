
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Extensions
{
    public static class ValidationServiceCollection
    {
        public static IServiceCollection AddValidators(
        this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(ValidationServiceCollection).Assembly);

            return services;
        }
    }
}
