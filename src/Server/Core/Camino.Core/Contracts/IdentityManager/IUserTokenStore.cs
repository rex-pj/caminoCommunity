using Camino.Core.Domain.Identities;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.IdentityManager
{
    public interface IUserTokenStore<TUser> where TUser : ApplicationUser
    {
        Task<ApplicationUserToken> FindTokenByValueAsync(TUser user, string value, string name);
        Task RemoveAuthenticationTokenByValueAsync(long userId, string value);
    }
}
