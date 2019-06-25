using Coco.Api.Framework.AccountIdentity.Entities;
using Coco.Api.Framework.Models;
using System;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IAccountManager<TUser> : IDisposable where TUser : class
    {
        IdentityOptions Options { get; set; }
        Task<ApiResult> CreateAsync(TUser user);
        Task<string> GetUserNameAsync(TUser user);
        Task<string> GetEmailAsync(TUser user);
        Task<TUser> FindByEmailAsync(string email);
        Task<TUser> FindByNameAsync(string userName);
        Task<string> GetUserIdAsync(TUser user);
        Task<ApiResult> CheckPasswordAsync(TUser user, string password);
        Task<TUser> GetLoggingUser(string userIdHased, string authenticatorToken);
        Task<TUser> GetFullByHashIdAsync(string userIdHased);
        Task<ApiResult> UpdateInfoAsync(TUser user);
        Task<ApiResult> UpdateInfoItemAsync(UpdatePerItemModel model, string token);
    }
}
