using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Camino.Infrastructure.Identity.Interfaces
{
    public interface ILoginManager<TUser> where TUser : class
    {
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task SignInWithClaimsAsync(TUser user, bool isPersistent, IEnumerable<Claim> additionalClaims);
        Task SignOutAsync();
    }
}
