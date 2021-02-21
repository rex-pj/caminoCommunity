using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.IdentityManager
{
    public interface IUserEncryptionStore<TUser> where TUser: IdentityUser<long>
    {
        Task<string> EncryptUserId(long userId);
        Task<long> DecryptUserId(string userIdentityId);
    }
}
