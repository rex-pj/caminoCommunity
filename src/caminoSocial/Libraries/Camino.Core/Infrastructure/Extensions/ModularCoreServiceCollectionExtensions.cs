using Camino.Core.Models;
using Camino.Core.Modular.Contracts;
using Camino.Core.Modular;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Camino.Core.Infrastructure.Extensions
{
    public static class ModularCoreServiceCollectionExtensions
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
    }
}
