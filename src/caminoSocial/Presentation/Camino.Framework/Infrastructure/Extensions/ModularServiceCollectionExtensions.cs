using Camino.Core.Models;
using Camino.Core.Modular.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Camino.Framework.Infrastructure.Extensions
{
    public static class ModularServiceCollectionExtensions
    {
        public static IMvcBuilder AddModular(this IServiceCollection services, IList<ModuleInfo> modules)
        {
            var mvcBuilder = services.AddControllersWithViews().AddNewtonsoftJson();
            modules = modules.Where(x => x.ShortName.Contains("Content")).ToList();
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

            return mvcBuilder;
        }
    }
}
