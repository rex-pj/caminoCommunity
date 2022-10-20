using Camino.Core.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using System.Runtime.Loader;

namespace Camino.Infrastructure.Extensions.DependencyInjection
{
    public static class AutoRegisterServiceExtensions
    {
        private static readonly Type _transientDependencyType = typeof(ITransientDependency).GetTypeInfo();
        private static readonly Type _scopedDependencyType = typeof(IScopedDependency).GetTypeInfo();
        private static readonly Type _singletonDependencyType = typeof(ISingletonDependency).GetTypeInfo();

        public static void AddProjectDependencies(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var assemblies = GetProjectAssemblies(configuration, serviceProvider);
            foreach (var assembly in assemblies)
            {
                services.AddDependenciesFromAssembly(assembly);
            }
        }

        public static void AddModuleDependencies(this IServiceCollection services, Assembly currentAssembly)
        {
            var assemblies = GetModuleAssemblies(currentAssembly);
            foreach (var assembly in assemblies)
            {
                services.AddDependenciesFromAssembly(assembly);
            }
        }

        public static void AddDependenciesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var interfaceTypes = GetAssemblyInterfaceTypes(assembly);
            if (interfaceTypes != null && interfaceTypes.Any())
            {
                foreach (var interfaceType in interfaceTypes)
                {
                    var instanceTypes = GetInterfaceInstanceTypes(assembly, interfaceType);
                    if (instanceTypes == null || !instanceTypes.Any())
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

            var abstractTypes = GetAssemblyAbstractTypes(assembly);
            if (abstractTypes != null && abstractTypes.Any())
            {
                foreach (var abstractType in abstractTypes)
                {
                    var instanceTypes = GetAbstractInstanceTypes(assembly, abstractType);
                    if (abstractTypes == null || !abstractTypes.Any())
                    {
                        continue;
                    }

                    foreach (var initializerType in instanceTypes)
                    {
                        if (_transientDependencyType.IsAssignableFrom(initializerType))
                        {
                            services.AddTransient(abstractType, initializerType);
                        }
                        else if (_scopedDependencyType.IsAssignableFrom(initializerType))
                        {
                            services.AddScoped(abstractType, initializerType);
                        }
                        else if (_singletonDependencyType.IsAssignableFrom(initializerType))
                        {
                            services.AddSingleton(abstractType, initializerType);
                        }
                    }
                }
            }
        }

        private static IEnumerable<Type> GetInterfaceInstanceTypes(Assembly assembly, Type interfaceType)
        {
            var instanceTypes = assembly.GetTypes()
                .Where(x => _transientDependencyType.IsAssignableFrom(x) || _scopedDependencyType.IsAssignableFrom(x) || _singletonDependencyType.IsAssignableFrom(x))
                .Where(x => interfaceType.IsAssignableFrom(x) && x != interfaceType);

            return instanceTypes;
        }

        private static IEnumerable<Type> GetAbstractInstanceTypes(Assembly assembly, Type abstractType)
        {
            var instanceTypes = assembly.GetTypes()
                .Where(x => _transientDependencyType.IsAssignableFrom(x) || _scopedDependencyType.IsAssignableFrom(x) || _singletonDependencyType.IsAssignableFrom(x))
                .Where(x => abstractType.IsAssignableFrom(x) && x != abstractType);

            return instanceTypes;
        }

        private static IEnumerable<Type> GetAssemblyInterfaceTypes(Assembly assembly)
        {
            var interfaceTypes = assembly.GetTypes().SelectMany(x => x.GetInterfaces())
                .Where(x => !_transientDependencyType.IsEquivalentTo(x) && !_scopedDependencyType.IsEquivalentTo(x) && !_singletonDependencyType.IsEquivalentTo(x));

            return interfaceTypes;
        }

        private static IEnumerable<Type> GetAssemblyAbstractTypes(Assembly assembly)
        {
            var abstractTypes = assembly.GetTypes()
                .Where(x => !x.IsInterface)
                .Where(x => _transientDependencyType.IsAssignableFrom(x) || _scopedDependencyType.IsAssignableFrom(x) || _singletonDependencyType.IsAssignableFrom(x))
                .Select(x => x.BaseType)
                .Where(x => x?.IsAbstract ?? false);

            return abstractTypes;
        }

        private static IEnumerable<Assembly> GetProjectAssemblies(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            var projectAssemblies = AssemblyLoadContext.Default.Assemblies
                .Where(x => x.FullName.StartsWith("Camino.", StringComparison.OrdinalIgnoreCase)).ToList();

            var dependencieNames = DependencyContext.Default.RuntimeLibraries
                .Where(x => x.Name.StartsWith("Camino.", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Name);

            var missingAssemblies = dependencieNames.Where(x => !projectAssemblies.Any(a => a.GetName().Name.Equals(x)));
            if (missingAssemblies.Any())
            {
                var additionalAssemblies = missingAssemblies.Select(x => Assembly.Load(x));
                projectAssemblies.AddRange(additionalAssemblies);
            }

            return projectAssemblies;
        }

        private static IEnumerable<Assembly> GetModuleAssemblies(Assembly currentAssembly)
        {
            var moduleAssemblies = new List<Assembly> { currentAssembly };
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies()?
                .Where(x => x.FullName.StartsWith("Camino.", StringComparison.OrdinalIgnoreCase))?
                .Select(Assembly.Load);

            if (referencedAssemblies != null && referencedAssemblies.Any())
            {
                moduleAssemblies.AddRange(referencedAssemblies);
            }

            return moduleAssemblies;
        }
    }
}
