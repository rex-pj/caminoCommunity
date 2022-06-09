using Camino.Core.Domains.Users;
using Camino.Shared.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Users
{
    public interface IUserRepository
    {
        Task<bool> DeleteAsync(long id);
        Task<long> CreateAsync(User user);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByUsernameAsync(string username);
        Task<IList<User>> GetByIdsAsync(IEnumerable<long> ids);
        Task<bool> UpdateStatusAsync(long id, long updatedById, UserStatuses status);
        Task<User> FindByIdAsync(long id);
        Task<bool> PartialUpdateAsync(UserInfo userInfo, string fieldName, object value);
        Task<bool> UpdateAsync(User user);
    }
}
