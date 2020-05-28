using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Stores.Contracts
{
    public interface IUserEncryptionStore<TUser> where TUser: IdentityUser<long>
    {
        Task<string> EncryptUserId(long userId);
        Task<long> DecryptUserId(string userIdentityId);
    }
}
