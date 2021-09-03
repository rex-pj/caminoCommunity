using AutoMapper;
using Camino.Framework.Infrastructure.AutoMap;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Camino.Shared.Modularity;

namespace Camino.Framework.Infrastructure.Extensions
{
    public static class ModularServiceCollectionExtensions
    {
        public static void AddAutoMappingModular(this IServiceCollection services)
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
