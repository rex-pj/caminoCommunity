using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Camino.Core.Contracts.Modularity;
using Camino.Infrastructure.Modularity;

namespace Camino.Infrastructure.Infrastructure.Extensions
{
    public static class ModularCoreServiceCollectionExtensions
    {
        public static IMvcBuilder AddModular(this IMvcBuilder mvcBuilder)
        {
            var services = mvcBuilder.Services;
            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            var rootPath = Directory.GetParent(webHostEnvironment.ContentRootPath).Parent.FullName;
            var modulesPath = $"{rootPath}{configuration["Modular:Path"]}";
            var prefix = configuration["Modular:Prefix"];

            return AddModular(mvcBuilder, modulesPath, prefix);
        }

        public static IMvcBuilder AddModular(this IMvcBuilder mvcBuilder, string modulesPath, string prefix = null)
        {
            var services = mvcBuilder.Services;
            services.AddSingleton<IModularManager, ModularManager>();
            var serviceProvider = services.BuildServiceProvider();
            var modularManager = serviceProvider.GetRequiredService<IModularManager>();

            var modules = modularManager.LoadModules(modulesPath, prefix);
            services.AddSingleton(modules);

            var moduleStartupInterfaceType = typeof(IModuleStartup);
            foreach (var module in modules)
            {
                AddApplicationPart(mvcBuilder, module.Assembly);
                var moduleStartupType = module.Assembly.GetTypes().FirstOrDefault(x => moduleStartupInterfaceType.IsAssignableFrom(x));
                if (moduleStartupType != null && moduleStartupType != moduleStartupInterfaceType)
                {
                    var moduleStartup = Activator.CreateInstance(moduleStartupType) as IModuleStartup;
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
