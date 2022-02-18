using Camino.Core.Contracts.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Camino.Infrastructure.Extensions.DependencyInjection
{
    public static class AutoRegisterServiceExtensions
    {
        private static readonly Type _transientDependencyType = typeof(ITransientDependency).GetTypeInfo();
        private static readonly Type _scopedDependencyType = typeof(IScopedDependency).GetTypeInfo();
        private static readonly Type _singletonDependencyType = typeof(ISingletonDependency).GetTypeInfo();
        public static void AddDependencyServices(this IServiceCollection services, string[] projectNames)
        {
            var assemblies = GetProjectsAssemblies(projectNames);
            foreach (var assembly in assemblies)
            {
                var interfaceTypes = GetAssemblyInterfaceTypes(assembly);
                if (interfaceTypes == null || !interfaceTypes.Any())
                {
                    continue;
                }

                foreach (var interfaceType in interfaceTypes)
                {
                    var instanceTypes = GetInstanceTypes(assembly, interfaceType);
                    if (instanceTypes == null && !instanceTypes.Any())
                    {
                        continue;
                    }

                    foreach (var initializerType in instanceTypes)
                    {
                        if (_transientDependencyType.IsAssignableFrom(initializerType))
                        {
                            services.AddTransient(interfaceType, initializerType);
                        }
                        else if (_scopedDependencyType.IsAssignableFrom(initializerType))
                        {
                            services.AddScoped(interfaceType, initializerType);
                        }
                        else if (_singletonDependencyType.IsAssignableFrom(initializerType))
                        {
                            services.AddSingleton(interfaceType, initializerType);
                        }
                    }
                }
            }
        }

        private static IEnumerable<Type> GetInstanceTypes(Assembly assembly, Type interfaceType)
        {
            var instanceTypes = assembly.GetTypes()
                .Where(x => (_transientDependencyType.IsAssignableFrom(x) || _scopedDependencyType.IsAssignableFrom(x) || _singletonDependencyType.IsAssignableFrom(x)) &&
                interfaceType.IsAssignableFrom(x) && x != interfaceType);

            return instanceTypes;
        }

        private static IEnumerable<Type> GetAssemblyInterfaceTypes(Assembly assembly)
        {
            var interfaceTypes = assembly.GetTypes().SelectMany(x => x.GetInterfaces())
                .Where(x => !_transientDependencyType.IsEquivalentTo(x) && !_scopedDependencyType.IsEquivalentTo(x) && !_singletonDependencyType.IsEquivalentTo(x));

            return interfaceTypes;
        }

        private static IEnumerable<Assembly> GetProjectsAssemblies(string[] projectNames)
        {
            var entryAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = entryAssembly.GetReferencedAssemblies().Select(Assembly.Load);
            var assemblies = new List<Assembly> { entryAssembly }.Concat(referencedAssemblies)
                .Where(x => projectNames.Contains(x.GetName().Name));

            return assemblies;
        }
    }
}
