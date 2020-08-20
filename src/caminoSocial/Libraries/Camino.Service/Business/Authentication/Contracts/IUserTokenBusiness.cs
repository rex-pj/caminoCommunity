using Camino.Service.Data.Identity;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authentication.Contracts
{
    public interface IUserTokenBusiness
    {
        void Add(UserTokenResult userTokenDto);
        void Remove(UserTokenResult userTokenDto);
        Task<UserTokenResult> FindAsync(long userId, string loginProvider, string name);
    }
}
