using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Camino.Framework.Providers.Implementation
{
    public static class ModularManager
    {
        private static ConcurrentDictionary<Type, IEnumerable<Type>> _types;

        public static IEnumerable<Assembly> Assemblies { get; private set; }

        public static void SetAssemblies(IEnumerable<Assembly> assemblies)
        {
            Assemblies = assemblies;
            _types = new ConcurrentDictionary<Type, IEnumerable<Type>>();
        }

        public static IEnumerable<T> GetInstances<T>(bool useCaching = false)
        {
            return GetInstances<T>(null, useCaching, new object[] { });
        }

        public static IEnumerable<T> GetInstances<T>(Func<Assembly, bool> predicate, bool useCaching = false, params object[] args)
        {
            List<T> instances = new List<T>();

            foreach (Type implementation in GetImplementations<T>(predicate, useCaching))
            {
                if (!implementation.GetTypeInfo().IsAbstract)
                {
                    T instance = (T)Activator.CreateInstance(implementation, args);

                    instances.Add(instance);
                }
            }

            return instances;
        }

        public static IEnumerable<Type> GetImplementations<T>(Func<Assembly, bool> predicate, bool useCaching = false)
        {
            Type type = typeof(T);

            if (useCaching && _types.ContainsKey(type))
            {
                return _types[type];
            }

            List<Type> implementations = new List<Type>();

            foreach (Assembly assembly in GetAssemblies(predicate))
            {
                foreach (Type exportedType in assembly.GetExportedTypes())
                {
                    if (type.GetTypeInfo().IsAssignableFrom(exportedType) && exportedType.GetTypeInfo().IsClass)
                    {
                        implementations.Add(exportedType);
                    }
                }
                    
            }

            if (useCaching)
            {
                _types[type] = implementations;
            }

            return implementations;
        }

        private static IEnumerable<Assembly> GetAssemblies(Func<Assembly, bool> predicate)
        {
            if (predicate == null)
            {
                return Assemblies;
            }
                
            return Assemblies.Where(predicate);
        }
    }
}
