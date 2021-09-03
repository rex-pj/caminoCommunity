using Camino.Shared.Requests.Authentication;
using Camino.Shared.Results.Authentication;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Authentication
{
    public interface IUserTokenRepository
    {
        void Create(UserTokenRequest request);
        Task RemoveAsync(UserTokenRequest request);
        Task<UserTokenResult> FindAsync(long userId, string loginProvider, string name);
        Task<UserTokenResult> FindByValueAsync(long userId, string value, string name);
        Task RemoveByValueAsync(UserTokenRequest request);
    }
}
