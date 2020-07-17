using Coco.Business.Dtos.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
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
