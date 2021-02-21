using Camino.Shared.Requests.Authentication;
using Camino.Shared.Results.Authentication;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Authentication
{
    public interface IUserTokenRepository
    {
        void Create(UserTokenRequest request);
        void Remove(UserTokenRequest request);
        Task<UserTokenResult> FindAsync(long userId, string loginProvider, string name);
    }
}
