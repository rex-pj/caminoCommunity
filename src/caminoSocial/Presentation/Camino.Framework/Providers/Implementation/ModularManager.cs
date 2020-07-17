using Camino.Core.Infrastructure;
using Camino.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Camino.Framework.Providers.Implementation
{
    public class ModularManager
    {
        public IList<ModuleInfo> LoadModules(string pluginsPath, string prefix = null)
        {
            var moduleRootFolder = new DirectoryInfo(pluginsPath);
            var moduleFolders = moduleRootFolder.GetDirectories();
            if (prefix != null)
            {
                moduleFolders = moduleFolders.Where(x => x.Name.Contains(prefix)).ToArray();
            }

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
