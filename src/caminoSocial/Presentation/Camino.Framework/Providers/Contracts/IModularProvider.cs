using Camino.Core.Models;
using System.Collections.Generic;

namespace Camino.Framework.Providers.Contracts
{
    public interface IModularProvider
    {
        IList<ModuleInfo> LoadModules(string pluginsPath, string prefix = null);
    }
}
