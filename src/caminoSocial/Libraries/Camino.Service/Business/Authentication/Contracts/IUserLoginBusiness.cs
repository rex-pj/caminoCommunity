using Camino.Service.Projections.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authentication.Contracts
{
    public interface IUserLoginBusiness
    {
        void Add(UserLoginRequest userLoginRequest);
        void Remove(UserLoginRequest userLoginRequest);
        Task<UserLoginRequest> FindAsync(long userId, string loginProvider, string providerKey);
        Task<UserLoginRequest> FindAsync(string loginProvider, string providerKey);
        Task<IList<UserLoginRequest>> GetByUserIdAsync(long userId);
    }
}
