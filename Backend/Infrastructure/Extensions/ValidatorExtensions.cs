using Application.Features.Auth.DTOs;
using Application.Features.Auth.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ValidatorExtensions
    {
        public static IServiceCollection AddValidators(
        this IServiceCollection services)
        {
            services.AddScoped<
                IValidator<LoginRequestDto>,
                LoginRequestValidator>();

            return services;
        }
    }
}
