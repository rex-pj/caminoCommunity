using Camino.Shared.General;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Navigations;
using Camino.Shared.Results.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Services.Navigations
{
    public interface IShortcutService
    {
        Task<int> CreateAsync(ShortcutModifyRequest request);
        Task<ShortcutResult> FindAsync(IdRequestFilter<int> filter);
        Task<ShortcutResult> FindByNameAsync(string name);
        Task<BasePageList<ShortcutResult>> GetAsync(ShortcutFilter filter);
        IList<SelectOption> GetShortcutTypes(ShortcutTypeFilter filter);
        Task<bool> UpdateAsync(ShortcutModifyRequest request);
        Task<bool> DeactivateAsync(ShortcutModifyRequest request);
        Task<bool> ActiveAsync(ShortcutModifyRequest request);
        Task<bool> DeleteAsync(int id);
        IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "");
    }
}