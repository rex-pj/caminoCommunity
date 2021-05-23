using Camino.Shared.General;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Navigations;
using Camino.Shared.Results.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Navigations
{
    public interface IShortcutRepository
    {
        Task<int> CreateAsync(ShortcutModifyRequest request);
        Task<ShortcutResult> FindAsync(int id);
        Task<ShortcutResult> FindByNameAsync(string name);
        Task<BasePageList<ShortcutResult>> GetAsync(ShortcutFilter filter);
        IList<SelectOption> GetShortcutTypes(ShortcutTypeFilter filter);
        Task<bool> UpdateAsync(ShortcutModifyRequest request);
    }
}