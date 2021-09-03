using Camino.Shared.General;
using Camino.Shared.Requests.Filters;
using System.Collections.Generic;

namespace Camino.Core.Contracts.Repositories.Navigations
{
    public interface IShortcutTypeRepository
    {
        IList<SelectOption> Get(ShortcutTypeFilter filter);
    }
}
