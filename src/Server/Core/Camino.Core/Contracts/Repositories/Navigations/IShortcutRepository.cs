using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Navigations;
using Camino.Shared.Results.PageList;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Navigations
{
    public interface IShortcutRepository
    {
        Task<int> CreateAsync(ShortcutModifyRequest request);
        Task<ShortcutResult> FindAsync(IdRequestFilter<int> filter);
        Task<ShortcutResult> FindByNameAsync(string name);
        Task<BasePageList<ShortcutResult>> GetAsync(ShortcutFilter filter);
        Task<bool> UpdateAsync(ShortcutModifyRequest request);
        Task<bool> DeactivateAsync(ShortcutModifyRequest request);
        Task<bool> ActiveAsync(ShortcutModifyRequest request);
        Task<bool> DeleteAsync(int id);
    }
}