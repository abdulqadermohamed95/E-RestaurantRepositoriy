using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ERestaurant.Application.Mapping
{
    public static class AutoMapperConfig
    {
        /// <summary>
        /// Here it is used to make the autoMapper not need to be added every time when making a new one, and it is sufficient to make one place appear in it
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
