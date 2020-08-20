using Camino.Service.Data.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authentication.Contracts
{
    public interface IUserLoginBusiness
    {
        void Add(UserLoginDto userLoginDto);
        void Remove(UserLoginDto userLoginDto);
        Task<UserLoginDto> FindAsync(long userId, string loginProvider, string providerKey);
        Task<UserLoginDto> FindAsync(string loginProvider, string providerKey);
        Task<IList<UserLoginDto>> GetByUserIdAsync(long userId);
    }
}
