using Coco.Api.Framework.AccountIdentity.Entities;
using System;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IAccountManager<TUser> : IDisposable where TUser : class
    {
        IdentityOptions Options { get; set; }
        Task<IdentityResult> CreateAsync(TUser user);
        Task<string> GetUserNameAsync(TUser user);
        Task<string> GetEmailAsync(TUser user);
        Task<TUser> FindByEmailAsync(string email);
        Task<TUser> FindByNameAsync(string userName);
        Task<string> GetUserIdAsync(TUser user);
        Task<LoginResult> CheckPasswordAsync(TUser user, string password);
        Task<TUser> FindByTokenAsync(string authenticatorToken, string userIdHased);
    }
}
