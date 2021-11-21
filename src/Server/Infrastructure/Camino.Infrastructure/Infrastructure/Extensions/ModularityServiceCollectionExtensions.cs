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
using System.Collections.Generic;
using Camino.Shared.Modularity;

namespace Camino.Infrastructure.Infrastructure.Extensions
{
    public static class ModularCoreServiceCollectionExtensions
    {
        private const string _modularPath = "Modular:Path";
        private const string _modularPrefix = "Modular:Prefix";

        public static IMvcBuilder AddModular(this IMvcBuilder mvcBuilder)
        {
            var services = mvcBuilder.Services;
            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            var modules = services.GetModules(configuration, webHostEnvironment);
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

        private static IList<ModuleInfo> GetModules(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            services.AddSingleton<IModularManager, ModularManager>();

            var rootPath = Directory.GetParent(webHostEnvironment.ContentRootPath).Parent.Parent.FullName;
            var modulesPath = $"{rootPath}{configuration[_modularPath]}";
            var prefix = configuration[_modularPrefix];

            var modularManager = services.BuildServiceProvider()
                .GetRequiredService<IModularManager>();

            var modules = modularManager.LoadModules(modulesPath, prefix);
            services.AddSingleton(modules);

            return modules;
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
