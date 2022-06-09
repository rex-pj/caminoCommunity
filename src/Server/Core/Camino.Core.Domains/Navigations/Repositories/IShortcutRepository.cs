using Camino.Core.Domains.Navigations;

namespace Camino.Core.Contracts.Repositories.Navigations
{
    public interface IShortcutRepository
    {
        Task<int> CreateAsync(Shortcut request);
        Task<Shortcut> FindAsync(int id);
        Task<Shortcut> FindByNameAsync(string name);
        Task<bool> UpdateAsync(Shortcut request);
        Task<bool> DeleteAsync(int id);
    }
}