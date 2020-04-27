using Coco.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface ILoginManager<TUser> where TUser : class
    {
        Task<ICommonResult> LoginAsync(string userName, string password, bool canRemember = true);
        Task<bool> LogoutAsync(ApplicationUser user = null);
    }
}
