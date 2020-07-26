using Camino.Core.Infrastructure;
using Camino.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Camino.Framework.Providers.Implementation
{
    public class ModularManager
    {
        public List<ModuleInfo> LoadModules(string pluginsPath)
        {
            var moduleRootFolder = new DirectoryInfo(pluginsPath);
            var moduleFolders = moduleRootFolder.GetDirectories();

            var modules = new List<ModuleInfo>();

            foreach (var moduleFolder in moduleFolders)
            {
                var binFolder = new DirectoryInfo(Path.Combine(moduleFolder.FullName, "bin"));
                if (!binFolder.Exists)
                {
                    continue;
                }

                var dllFiles = binFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories);
                foreach (var file in dllFiles)
                {
                    Assembly assembly = null;
                    try
                    {
                        assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                    }
                    catch (FileLoadException)
                    {
                        assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));

                        if (assembly == null)
                        {
                            throw;
                        }
                    }

                    if (assembly.FullName.Contains(moduleFolder.Name))
                    {
                        modules.Add(new ModuleInfo { Name = moduleFolder.Name, Assembly = assembly, Path = moduleFolder.FullName });
                    }
                }
            }

            Singleton<IList<ModuleInfo>>.Instance = modules;
            return modules;
        }
    }
}
