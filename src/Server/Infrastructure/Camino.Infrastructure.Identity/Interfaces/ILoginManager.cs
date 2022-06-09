using Microsoft.AspNetCore.Identity;

namespace Camino.Infrastructure.Identity.Interfaces
{
    public interface ILoginManager<TUser> where TUser : class
    {
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task SignOutAsync();
    }
}
