using Camino.Core.Infrastructure;
using Camino.Core.Models;
using Camino.Core.Modular.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Camino.Core.Modular
{
    public class ModularManager : IModularManager
    {
        public IList<ModuleInfo> LoadModules(string modulesPath, string prefix = null)
        {
            var moduleRootFolder = new DirectoryInfo(modulesPath);
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
                    if (modules.Any(x => x.Path == moduleFolder.FullName))
                    {
                        continue;
                    }

                    var module = GetModuleByFile(file, moduleFolder, binFolder);
                    if (module != null)
                    {
                        modules.Add(module);
                    }
                }
            }

            return modules;
        }

        private ModuleInfo GetModuleByFile(FileSystemInfo fileSystemInfo, DirectoryInfo moduleFolder, DirectoryInfo binFolder)
        {
            Assembly assembly;
            try
            {
                assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fileSystemInfo.FullName);
            }
            catch (FileLoadException)
            {
                assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(fileSystemInfo.Name)));
                if (assembly == null)
                {
                    throw;
                }
            }

            if (assembly.FullName.Contains(moduleFolder.Name))
            {
                return new ModuleInfo { Name = moduleFolder.Name, Assembly = assembly, Path = moduleFolder.FullName };
            }

            return null;
        }
    }
}
