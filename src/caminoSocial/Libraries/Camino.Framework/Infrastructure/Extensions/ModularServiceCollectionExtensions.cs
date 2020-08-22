using AutoMapper;
using Camino.Service.AutoMap;
using Camino.Core.Infrastructure;
using Camino.Core.Models;
using Camino.Core.Modular.Implementations;
using Camino.Framework.Infrastructure.AutoMap;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Camino.Framework.Infrastructure.Extensions
{
    public static class ModularServiceCollectionExtensions
    {
        public static void AddAutoMappingModular(this IServiceCollection services)
        {
            var modules = Singleton<IList<ModuleInfo>>.Instance;
            var mapProfileType = typeof(Profile);
            var mappingProfileTypes = new List<Type>();
            mappingProfileTypes.Add(typeof(FrameworkMappingProfile));
            mappingProfileTypes.Add(typeof(IdentityMappingProfile));

            if (modules != null && modules.Any())
            {
                foreach (var module in modules)
                {
                    var mapProfiles = module.Assembly.GetTypes().Where(x => mapProfileType.IsAssignableFrom(x));
                    mappingProfileTypes.AddRange(mapProfiles);
                }
            }

            services.AddAutoMapper(mappingProfileTypes.ToArray());
        }

        public static void AddGraphQlModular(this IServiceCollection services)
        {
            var modules = Singleton<IList<ModuleInfo>>.Instance;
            if (modules != null && modules.Any())
            {
                services
                   .AddGraphQL(sp => SchemaBuilder.New()
                   .AddServices(sp)
                   .AddQueryType<QueryType>()
                   .AddMutationType<MutationType>()
                   .Create());
            }
        }
    }
}
