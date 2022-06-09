using Camino.Application.Contracts.AppServices.Navigations.Dtos;

namespace Camino.Application.Contracts.AppServices.Navigations
{
    public interface IShortcutAppService
    {
        Task<bool> ActiveAsync(int id, long updatedById);
        Task<int> CreateAsync(ShortcutModifyRequest request);
        Task<bool> DeactivateAsync(int id, long updatedById);
        Task<bool> DeleteAsync(int id);
        Task<ShortcutResult> FindAsync(IdRequestFilter<int> filter);
        Task<ShortcutResult> FindByNameAsync(string name);
        Task<BasePageList<ShortcutResult>> GetAsync(ShortcutFilter filter);
        IList<SelectOption> GetShortcutTypes(ShortcutTypeFilter filter);
        IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "");
        Task<bool> UpdateAsync(ShortcutModifyRequest request);
    }
}