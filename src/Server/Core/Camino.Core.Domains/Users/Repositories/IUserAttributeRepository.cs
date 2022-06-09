using Camino.Core.Domains.Users;

namespace Camino.Core.Contracts.Repositories.Users
{
    public interface IUserAttributeRepository
    {
        Task CreateAsync(IEnumerable<UserAttribute> userAttributes);
        Task<int> CreateAsync(UserAttribute userAttribute, bool needSaveChanges = false);
        Task<bool> DeleteAsync(long userId, string key);
        Task<bool> DeleteAsync(long userId, string key, string value);
        Task<IEnumerable<UserAttribute>> GetAsync(long userId);
        Task<UserAttribute> GetAsync(long userId, string key);
        Task<int> UpdateAsync(UserAttribute userAttribute, bool needSaveChanges = false);
    }
}
