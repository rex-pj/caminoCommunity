using Camino.Infrastructure.Identity.Core;

namespace Camino.Infrastructure.Identity.Interfaces
{
    public interface IUserEncryptionStore<TUser> where TUser: ApplicationUser
    {
        Task<string> EncryptUserIdAsync(long userId);
        Task<long> DecryptUserIdAsync(string userIdentityId);
        string EncryptUserId(long userId);
        long DecryptUserId(string userIdentityId);
    }
}
