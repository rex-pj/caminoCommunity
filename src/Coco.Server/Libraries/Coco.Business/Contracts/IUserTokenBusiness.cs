using Coco.Core.Dtos.Identity;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IUserTokenBusiness
    {
        void Add(UserTokenDto userTokenDto);
        void Remove(UserTokenDto userTokenDto);
        Task<UserTokenDto> FindAsync(long userId, string loginProvider, string name);
    }
}
