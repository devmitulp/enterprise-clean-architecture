using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Extensions
{
    public static class ApplicationServiceCollection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => {}, Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
