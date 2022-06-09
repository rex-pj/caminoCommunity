using Camino.Infrastructure.Identity.Core;

namespace Camino.Infrastructure.Identity.Interfaces
{
    public interface IUserEncryptionStore<TUser> where TUser: ApplicationUser
    {
        Task<string> EncryptUserId(long userId);
        Task<long> DecryptUserId(string userIdentityId);
    }
}
