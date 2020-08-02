using AutoMapper;
using Camino.Business.AutoMap;
using Camino.Core.Infrastructure;
using Camino.Core.Models;
using Camino.Core.Modular.Contracts;
using Camino.Core.Modular.Implementations;
using Camino.Framework.Infrastructure.AutoMap;
using Camino.Core.Modular;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Camino.Framework.Infrastructure.Extensions
{
    public static class ModularServiceCollectionExtensions
    {
        public static IMvcBuilder AddModular(this IMvcBuilder mvcBuilder, string modulesPath, string prefix = null)
        {
            var services = mvcBuilder.Services;
            services.AddSingleton<IModularManager, ModularManager>();
            var serviceProvider = services.BuildServiceProvider();
            var modularManager = serviceProvider.GetRequiredService<IModularManager>();
            modularManager.LoadModules(modulesPath, prefix);

            var modules = Singleton<IList<ModuleInfo>>.Instance;
            var pluginStartupInterfaceType = typeof(IPluginStartup);
            foreach (var module in modules)
            {
                AddApplicationPart(mvcBuilder, module.Assembly);
                var pluginStartupType = module.Assembly.GetTypes().FirstOrDefault(x => pluginStartupInterfaceType.IsAssignableFrom(x));
                if (pluginStartupType != null && pluginStartupType != pluginStartupInterfaceType)
                {
                    var moduleStartup = Activator.CreateInstance(pluginStartupType) as IPluginStartup;
                    moduleStartup.ConfigureServices(services);
                }
            }

            return mvcBuilder;
        }

        private static void AddApplicationPart(IMvcBuilder mvcBuilder, Assembly assembly)
        {
            var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
            foreach (var part in partFactory.GetApplicationParts(assembly))
            {
                mvcBuilder.PartManager.ApplicationParts.Add(part);
            }

            var relatedAssemblies = RelatedAssemblyAttribute.GetRelatedAssemblies(assembly, throwOnError: false);
            foreach (var relatedAssembly in relatedAssemblies)
            {
                partFactory = ApplicationPartFactory.GetApplicationPartFactory(relatedAssembly);
                foreach (var part in partFactory.GetApplicationParts(relatedAssembly))
                {
                    mvcBuilder.PartManager.ApplicationParts.Add(part);
                }
            }
        }

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
