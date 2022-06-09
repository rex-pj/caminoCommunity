namespace Camino.Core.Domains.Authorization.Repositories
{
    public interface IUserRoleRepository
    {
        Task<IList<UserRole>> GetListAsync(long userId);
        Task<UserRole> FindAsync(long userId, long roleId);
        Task<bool> RemoveAsync(long roleId, long userId);
        Task<bool> CreateAsync(UserRole userRoleRequest);
    }
}
