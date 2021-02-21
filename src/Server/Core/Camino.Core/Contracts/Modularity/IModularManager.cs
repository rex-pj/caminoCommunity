using Camino.Shared.Modularity;
using System.Collections.Generic;

namespace Camino.Core.Contracts.Modularity
{
    public interface IModularManager
    {
        IList<ModuleInfo> LoadModules(string modulesPath, string prefix = null);
    }
}
