using Camino.Core.Infrastructure;
using Camino.Core.Models;
using Camino.Core.Modular.Contracts;
using Camino.Core.Modular.Implementations;
using Camino.Framework.Providers.Implementation;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Camino.Framework.Infrastructure.Extensions
{
    public static class ModularServiceCollectionExtensions
    {
        public static IMvcBuilder AddModular(this IServiceCollection services, string modulesPath, string prefix = null)
        {
            services.AddSingleton<ModularManager>();
            var serviceProvider = services.BuildServiceProvider();
            var modularManager = serviceProvider.GetRequiredService<ModularManager>();
            modularManager.LoadModules(modulesPath, prefix);

            var modules = Singleton<IList<ModuleInfo>>.Instance;
            var mvcBuilder = services.AddControllersWithViews().AddNewtonsoftJson();
            var pluginStartupInterfaceType = typeof(IPluginStartup);
            foreach (var module in modules)
            {
                mvcBuilder.AddApplicationPart(module.Assembly);

                var pluginStartupType = module.Assembly.GetTypes().FirstOrDefault(x => pluginStartupInterfaceType.IsAssignableFrom(x));
                if (pluginStartupType != null && pluginStartupType != pluginStartupInterfaceType)
                {
                    var moduleStartup = Activator.CreateInstance(pluginStartupType) as IPluginStartup;
                    moduleStartup.ConfigureServices(services);
                }
            }

            AddGraphQlModules(services);
            return mvcBuilder;
        }

        public static void AddGraphQlModules(this IServiceCollection services)
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
