namespace Camino.Core.Domains.Authentication.Repositories
{
    public interface IUserLoginRepository
    {
        Task<long> CreateAsync(UserLogin userLogin);
        Task<UserLogin> FindAsync(long userId, string loginProvider, string providerKey);
        Task<UserLogin> FindAsync(string loginProvider, string providerKey);
        Task<IList<UserLogin>> GetByUserIdAsync(long userId);
        Task<bool> RemoveAsync(string loginProvider, string providerKey, string providerDisplayName, long userId);
    }
}
