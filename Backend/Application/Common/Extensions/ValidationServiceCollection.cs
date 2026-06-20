
using Application.Features.Auth.DTOs;
using Application.Features.Auth.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Extensions
{
    public static class ValidationServiceCollection
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
