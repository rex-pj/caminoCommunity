using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface IUserManager<TUser> : IDisposable where TUser : class
    {
        Task<IdentityResult> CreateAsync(TUser user);
        Task<IdentityResult> CreateAsync(TUser user, string password);
        Task<string> GetAuthenticationTokenAsync(TUser user, string loginProvider, string tokenName);
        Task<bool> CheckPasswordAsync(TUser user, string password);
        Task<TUser> FindByNameAsync(string userName);
        Task<IdentityResult> RemoveAuthenticationTokenAsync(TUser user, string loginProvider, string tokenName);
        Task<string> GenerateUserTokenAsync(TUser user, string tokenProvider, string purpose);
        Task<IdentityResult> SetAuthenticationTokenAsync(TUser user, string loginProvider, string tokenName, string tokenValue);
    }
}
