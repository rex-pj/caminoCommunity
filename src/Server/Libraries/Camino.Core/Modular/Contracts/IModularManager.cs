using Camino.Core.Models;
using System.Collections.Generic;

namespace Camino.Core.Modular.Contracts
{
    public interface IModularManager
    {
        IList<ModuleInfo> LoadModules(string modulesPath, string prefix = null);
    }
}
