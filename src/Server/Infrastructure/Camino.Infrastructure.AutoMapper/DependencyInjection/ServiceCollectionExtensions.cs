using AutoMapper;
using Camino.Infrastructure.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.Infrastructure.AutoMapper.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AutoRegisterAutoMapper(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var modules = serviceProvider.GetRequiredService<IList<ModuleInfo>>();
            var mappingProfileTypes = new List<Type>
            {
                typeof(FrameworkMappingProfile),
            };

            if (modules != null && modules.Any())
            {
                var mapProfileType = typeof(Profile);
                foreach (var module in modules)
                {
                    var mapProfiles = module.Assembly.GetTypes().Where(x => mapProfileType.IsAssignableFrom(x));
                    mappingProfileTypes.AddRange(mapProfiles);
                }
            }

            services.AddAutoMapper(mappingProfileTypes.ToArray());
        }
    }
}
