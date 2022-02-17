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
        public static void AddDependencyServices(this IServiceCollection services, string[] projectNames)
        {
            var assemblies = GetProjectsAssemblies(projectNames);
            var transientType = typeof(ITransientDependency).GetTypeInfo();
            var scopedType = typeof(IScopedDependency).GetTypeInfo();
            var singletonType = typeof(ISingletonDependency).GetTypeInfo();
            foreach (var assembly in assemblies)
            {
                var interfaceTypes = GetAssemblyInterfaceTypes(assembly);
                foreach (var interfaceType in interfaceTypes)
                {
                    var instanceTypes = GetInstanceTypes(assembly, interfaceType);
                    foreach (var initializerType in instanceTypes)
                    {
                        if (transientType.IsAssignableFrom(initializerType))
                        {
                            services.AddTransient(interfaceType, initializerType);
                        }
                        else if (scopedType.IsAssignableFrom(initializerType))
                        {
                            services.AddScoped(interfaceType, initializerType);
                        }
                        else if (singletonType.IsAssignableFrom(initializerType))
                        {
                            services.AddSingleton(interfaceType, initializerType);
                        }                        
                    }
                }
            }            
        }

        private static IEnumerable<Type> GetInstanceTypes(Assembly assembly, Type interfaceType)
        {
            var instanceTypes = assembly.GetTypes().Where(x => interfaceType.IsAssignableFrom(x) && x != interfaceType);
            return instanceTypes;
        }

        private static IEnumerable<Type> GetAssemblyInterfaceTypes(Assembly assembly)
        {
            var transientType = typeof(ITransientDependency).GetTypeInfo();
            var scopedType = typeof(IScopedDependency).GetTypeInfo();
            var singletonType = typeof(ISingletonDependency).GetTypeInfo();
            var interfaceTypes = assembly.GetTypes().SelectMany(x => x.GetInterfaces())
                .Where(x => !transientType.IsEquivalentTo(x) && !scopedType.IsEquivalentTo(x) && !singletonType.IsEquivalentTo(x));

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
