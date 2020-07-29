using Camino.Framework.Providers.Contracts;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Camino.Framework.Providers.Implementation
{
    public class AssemblyProvider : IAssemblyProvider
    {
        public Func<Assembly, bool> IsCandidateAssembly { get; set; }

        public Func<Library, bool> IsCandidateCompilationLibrary { get; set; }

        public AssemblyProvider(IServiceProvider serviceProvider)
        {
            this.IsCandidateAssembly = assembly =>
              !assembly.FullName.StartsWith("System", StringComparison.OrdinalIgnoreCase) &&
              !assembly.FullName.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase);

            this.IsCandidateCompilationLibrary = library =>
              !library.Name.StartsWith("mscorlib", StringComparison.OrdinalIgnoreCase) &&
              !library.Name.StartsWith("netstandard", StringComparison.OrdinalIgnoreCase) &&
              !library.Name.StartsWith("System", StringComparison.OrdinalIgnoreCase) &&
              !library.Name.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase) &&
              !library.Name.StartsWith("WindowsBase", StringComparison.OrdinalIgnoreCase);
        }

        public IEnumerable<Assembly> GetAssemblies(string path, bool includingSubpaths)
        {
            List<Assembly> assemblies = new List<Assembly>();

            GetAssembliesFromDependencyContext(assemblies);
            GetAssembliesFromPath(assemblies, path, includingSubpaths);
            return assemblies;
        }

        private void GetAssembliesFromDependencyContext(List<Assembly> assemblies)
        {
            foreach (CompilationLibrary compilationLibrary in DependencyContext.Default.CompileLibraries)
            {
                if (IsCandidateCompilationLibrary(compilationLibrary))
                {
                    try
                    {
                        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(compilationLibrary.Name));

                        if (!assemblies.Any(a => string.Equals(a.FullName, assembly.FullName, StringComparison.OrdinalIgnoreCase)))
                        {
                            assemblies.Add(assembly);
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private void GetAssembliesFromPath(List<Assembly> assemblies, string path, bool includingSubpaths)
        {
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                foreach (string extensionPath in Directory.EnumerateFiles(path, "*.dll"))
                {
                    try
                    {
                        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(extensionPath);

                        if (IsCandidateAssembly(assembly) && !assemblies.Any(a => string.Equals(a.FullName, assembly.FullName, StringComparison.OrdinalIgnoreCase)))
                        {
                            assemblies.Add(assembly);
                        }
                    }
                    catch (Exception e)
                    {
                        
                    }
                    
                }

                if (includingSubpaths)
                {
                    foreach (string subpath in Directory.GetDirectories(path))
                    {
                        GetAssembliesFromPath(assemblies, subpath, includingSubpaths);
                    }
                }      
            }
        }
    }
}
