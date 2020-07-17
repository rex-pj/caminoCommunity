using Camino.Framework.Providers.Contracts;
using Camino.Framework.Providers.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Camino.Framework.Infrastructure.Extensions
{
    public static class ModularServiceCollectionExtensions
    {
        public static void AddExtCore(this IServiceCollection services)
        {
            services.AddModular(null);
        }

        public static void AddModular(this IServiceCollection services, string extensionsPath)
        {
            services.AddModular(extensionsPath, false, new AssemblyProvider(services.BuildServiceProvider()));
        }

        public static void AddModular(this IServiceCollection services, string extensionsPath, bool includingSubpaths, IAssemblyProvider assemblyProvider)
        {
            DiscoverAssemblies(assemblyProvider, extensionsPath, includingSubpaths);

            var serviceProvider = services.BuildServiceProvider();
            foreach (var action in ModularManager.GetInstances<IConfigureServicesAction>().OrderBy(a => a.Priority))
            {
                action.Execute(services, serviceProvider);
                serviceProvider = services.BuildServiceProvider();
            }
        }

        private static void DiscoverAssemblies(IAssemblyProvider assemblyProvider, string extensionsPath, bool includingSubpaths)
        {
            ModularManager.SetAssemblies(assemblyProvider.GetAssemblies(extensionsPath, includingSubpaths));
        }
    }
}
