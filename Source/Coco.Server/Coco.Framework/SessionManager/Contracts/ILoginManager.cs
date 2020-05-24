using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface ILoginManager<TUser> where TUser : class
    {
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task<bool> LogoutAsync(TUser user = null);
    }
}
