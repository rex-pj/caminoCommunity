using Camino.Service.Data.Identity;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authentication.Contracts
{
    public interface IUserTokenBusiness
    {
        void Add(UserTokenProjection userTokenRequest);
        void Remove(UserTokenProjection userTokenRequest);
        Task<UserTokenProjection> FindAsync(long userId, string loginProvider, string name);
    }
}
