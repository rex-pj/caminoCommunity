using Coco.Entities.Dtos.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IUserLoginBusiness
    {
        void Add(UserLoginDto userTokenDto);
        void Remove(UserLoginDto userTokenDto);
        Task<UserLoginDto> FindAsync(long userId, string loginProvider, string providerKey);
        Task<UserLoginDto> FindAsync(string loginProvider, string providerKey);
        Task<IList<UserLoginDto>> GetByUserIdAsync(long userId);
    }
}
