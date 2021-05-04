using Camino.Core.Domain.Identities;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.IdentityManager
{
    public interface IUserEncryptionStore<TUser> where TUser: ApplicationUser
    {
        Task<string> EncryptUserId(long userId);
        Task<long> DecryptUserId(string userIdentityId);
    }
}
