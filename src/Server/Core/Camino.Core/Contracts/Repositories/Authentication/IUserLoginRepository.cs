using Camino.Shared.Requests.Authentication;
using Camino.Shared.Results.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Authentication
{
    public interface IUserLoginRepository
    {
        void Create(UserLoginRequest userLoginRequest);
        void Remove(UserLoginRequest userLoginRequest);
        Task<UserLoginResult> FindAsync(long userId, string loginProvider, string providerKey);
        Task<UserLoginResult> FindAsync(string loginProvider, string providerKey);
        Task<IList<UserLoginResult>> GetByUserIdAsync(long userId);
    }
}
