using Camino.Core.Contracts.Modularity;
using System.Reflection;
using System.Runtime.Loader;

namespace Camino.Infrastructure.Modularity
{
    public class ModularManager : IModularManager
    {
        public IList<ModuleInfo> LoadModules(string rootPath, ModuleListSettings settings)
        {
            if (settings.Modules == null || !settings.Modules.Any())
            {
                return new List<ModuleInfo>();
            }

            var moduleRootDirectory = new DirectoryInfo($"{rootPath}{settings.Path}");
            var moduleGroupDirectories = moduleRootDirectory.GetDirectories();
            if (moduleGroupDirectories == null || !moduleGroupDirectories.Any())
            {
                return new List<ModuleInfo>();
            }

            var srcDirectories = moduleGroupDirectories?.SelectMany(x => x.GetDirectories("src"));
            if (srcDirectories == null || !srcDirectories.Any())
            {
                return new List<ModuleInfo>();
            }

            var moduleNames = settings.Modules.Select(x => x.Name);
            var moduleDirectories = srcDirectories?.SelectMany(x => x.GetDirectories().Where(d => moduleNames.Contains(d.Name))).ToArray();
            if (moduleDirectories == null || !moduleDirectories.Any())
            {
                return new List<ModuleInfo>();
            }

            var modules = new List<ModuleInfo>();
            foreach (var moduleFolder in moduleDirectories)
            {
                var binFolder = new DirectoryInfo(Path.Combine(moduleFolder.FullName, "bin"));
                if (!binFolder.Exists)
                {
                    continue;
                }

                var dllFiles = binFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories);
                foreach (var dllFile in dllFiles)
                {
                    if (modules.Any(x => x.Path == moduleFolder.FullName))
                    {
                        continue;
                    }

                    var module = GetModuleByFile(dllFile, moduleFolder);
                    if (module != null)
                    {
                        modules.Add(module);
                    }
                }
            }

            return modules;
        }

        private ModuleInfo GetModuleByFile(FileSystemInfo fileSystemInfo, DirectoryInfo moduleFolder)
        {
            var assembly = GetModuleAssembly(fileSystemInfo);
            if (assembly == null || string.IsNullOrEmpty(assembly.FullName))
            {
                throw new FileLoadException($"No assembly of {fileSystemInfo.FullName} has been found!", fileSystemInfo.FullName);
            }

            if (!assembly.FullName.Contains(moduleFolder.Name))
            {
                return null;
            }

            var lastIndex = assembly.Location.LastIndexOf($"{moduleFolder.Name}.dll");
            var moduleBinPath = assembly.Location.Remove(lastIndex);
            return new ModuleInfo { Name = moduleFolder.Name, Assembly = assembly, Path = moduleBinPath };
        }

        private Assembly GetModuleAssembly(FileSystemInfo fileSystemInfo)
        {
            try
            {
                return AssemblyLoadContext.Default.LoadFromAssemblyPath(fileSystemInfo.FullName);
            }
            catch (FileLoadException)
            {
                return Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(fileSystemInfo.Name)));
            }
        }
    }
}
