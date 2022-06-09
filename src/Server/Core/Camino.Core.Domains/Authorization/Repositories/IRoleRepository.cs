namespace Camino.Core.Domains.Authorization.Repositories
{
    public interface IRoleRepository
    {
        Task<long> CreateAsync(Role role);
        Task<bool> DeleteAsync(long id);
        Task<Role> FindAsync(long id);
        Task<Role> FindByNameAsync(string name);
        Task<bool> UpdateAsync(Role role);
    }
}