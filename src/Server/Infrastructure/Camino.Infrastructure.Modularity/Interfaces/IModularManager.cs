using Camino.Infrastructure.Modularity;

namespace Camino.Core.Contracts.Modularity
{
    public interface IModularManager
    {
        IList<ModuleInfo> LoadModules(string modulesPath, string prefix = null);
    }
}
