using System.Collections.Generic;
using System.Reflection;

namespace Camino.Framework.Providers.Contracts
{
    public interface IAssemblyProvider
    {
        IEnumerable<Assembly> GetAssemblies(string path, bool includingSubpaths);
    }
}
