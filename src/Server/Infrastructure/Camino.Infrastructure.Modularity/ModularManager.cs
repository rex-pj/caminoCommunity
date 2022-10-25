﻿using Camino.Core.Contracts.Modularity;
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

            var srcDirectories = moduleGroupDirectories?.SelectMany(x => x.GetDirectories("src"));

            var moduleNames = settings?.Modules.Select(x => x.Name);
            var moduleDirectories = srcDirectories?.SelectMany(x => x.GetDirectories().Where(d => moduleNames.Contains(d.Name))).ToArray();

            var modules = new List<ModuleInfo>();
            foreach (var moduleFolder in moduleDirectories)
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
            var assembly = GetModuleAssembly(fileSystemInfo);
            if (assembly == null)
            {
                throw new FileLoadException($"No assembly of {fileSystemInfo.FullName} has been found!", fileSystemInfo.FullName);
            }

            if (!assembly.FullName.Contains(moduleFolder.Name))
            {
                return null;
            }

            return new ModuleInfo { Name = moduleFolder.Name, Assembly = assembly, Path = moduleFolder.FullName };
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
