using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.IdentityManager
{
    public interface ILoginManager<TUser> where TUser : class
    {
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task SignOutAsync();
    }
}
