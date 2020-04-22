using Coco.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface ILoginManager<TUser> where TUser : class
    {
        Task<IApiResult> LoginAsync(string userName, string password);
        Task<bool> LogoutAsync(string userIdentityId, string authenticationToken);
    }
}
