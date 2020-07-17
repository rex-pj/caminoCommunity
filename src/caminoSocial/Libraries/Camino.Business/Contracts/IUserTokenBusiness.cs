using Camino.Business.Dtos.Identity;
using System.Threading.Tasks;

namespace Camino.Business.Contracts
{
    public interface IUserTokenBusiness
    {
        void Add(UserTokenDto userTokenDto);
        void Remove(UserTokenDto userTokenDto);
        Task<UserTokenDto> FindAsync(long userId, string loginProvider, string name);
    }
}
