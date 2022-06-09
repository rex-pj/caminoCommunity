using Camino.Infrastructure.Identity.Core;

namespace Camino.Infrastructure.Identity.Interfaces
{
    public interface IUserTokenStore<TUser> where TUser : ApplicationUser
    {
        Task<ApplicationUserToken> FindTokenByValueAsync(TUser user, string value, string name);
        Task RemoveAuthenticationTokenByValueAsync(long userId, string value);
    }
}
