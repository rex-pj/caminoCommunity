using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Camino.Infrastructure.Infrastructure.Extensions
{
    public static class AutoRegisterServiceExtensions
    {
        public static void AddScopedServices(this IServiceCollection services, string[] projectNames, string[] interfaceNameSpaces)
        {
            var assemblies = GetProjectsAssemblies(projectNames);
            foreach (var assembly in assemblies)
            {
                var currentAssembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(assembly.FullName)));
                var interfaceTypes = GetAssemblyInterfaceTypes(currentAssembly, interfaceNameSpaces);
                foreach (var interfaceType in interfaceTypes)
                {
                    var instanceTypes = GetInstanceTypes(currentAssembly, interfaceType);
                    foreach (var initializerType in instanceTypes)
                    {
                        services.AddScoped(interfaceType, initializerType);
                    }
                }
            }
        }

        private static IEnumerable<Type> GetInstanceTypes(Assembly assembly, Type interfaceType)
        {
            var instanceTypes = assembly.GetTypes().Where(x => interfaceType.IsAssignableFrom(x) && x != interfaceType);
            return instanceTypes;
        }

        private static IEnumerable<Type> GetAssemblyInterfaceTypes(Assembly assembly, string[] interNameSpaces)
        {
            var interfaceTypes = assembly.GetTypes().SelectMany(x => x.GetInterfaces())
                .Where(x => interNameSpaces.Any(i => x.Namespace.StartsWith(i)));

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
