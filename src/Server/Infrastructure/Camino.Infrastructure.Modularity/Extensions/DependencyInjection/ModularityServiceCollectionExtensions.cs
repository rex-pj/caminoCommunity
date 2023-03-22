using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Camino.Core.Contracts.Modularity;
using Camino.Infrastructure.Modularity;
using Newtonsoft.Json;
using Camino.Infrastructure.Files.Contracts;
using System.Text;

namespace Camino.Infrastructure.Extensions.DependencyInjection
{
    public static class ModularCoreServiceCollectionExtensions
    {
        private const string _settingsPath = "Modular:Settings";

        public static IMvcBuilder AddModularManager(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.Services.AddSingleton<IModularManager, ModularManager>();
            return mvcBuilder;
        }

        public static IMvcBuilder AddModules(this IMvcBuilder mvcBuilder, IConfiguration configuration)
        {
            var services = mvcBuilder.Services;
            var serviceProvider = services.BuildServiceProvider();

            var modules = GetModules(configuration, serviceProvider);
            var moduleStartupInterfaceType = typeof(IModuleStartup);
            foreach (var module in modules)
            {
                AddApplicationParts(mvcBuilder, module.Assembly);
                var moduleStartupType = module.Assembly.GetTypes().FirstOrDefault(x => moduleStartupInterfaceType.IsAssignableFrom(x));
                if (moduleStartupType != null && moduleStartupType != moduleStartupInterfaceType)
                {
                    var moduleStartup = Activator.CreateInstance(moduleStartupType) as IModuleStartup;
                    moduleStartup.ConfigureServices(services);
                }
            }

            services.AddSingleton(modules);
            return mvcBuilder;
        }

        private static IList<ModuleInfo> GetModules(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            var fileProvider = serviceProvider.GetRequiredService<IFileProvider>();
            var projectPath = Directory.GetParent(webHostEnvironment.ContentRootPath);

            var moduleSettingsPath = Path.Combine(configuration[_settingsPath]);
            var settingsJson = fileProvider.ReadText(moduleSettingsPath, Encoding.UTF8);

            var settings = JsonConvert.DeserializeObject<ModuleListSettings>(settingsJson);
            var modularManager = serviceProvider.GetRequiredService<IModularManager>();

            var modules = modularManager.LoadModules(projectPath.FullName, settings);
            return modules;
        }

        private static void AddApplicationParts(IMvcBuilder mvcBuilder, Assembly assembly)
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
