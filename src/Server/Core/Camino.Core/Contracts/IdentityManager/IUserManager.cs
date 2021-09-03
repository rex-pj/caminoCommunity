using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Camino.Core.Domain.Identities;
using Microsoft.AspNetCore.Identity;

namespace Camino.Core.Contracts.IdentityManager
{
    public interface IUserManager<TUser> : IDisposable where TUser : class
    {
        Task<IdentityResult> CreateAsync(TUser user, string password);
        Task<TUser> FindByNameAsync(string userName);
        Task<IdentityResult> RemoveAuthenticationTokenAsync(TUser user, string loginProvider, string tokenName);
        Task<string> GenerateUserTokenAsync(TUser user, string tokenProvider, string purpose);
        Task<IdentityResult> SetAuthenticationTokenAsync(TUser user, string loginProvider, string tokenName, string tokenValue);
        Task<string> GetAuthenticationTokenAsync(TUser user, string loginProvider, string tokenName);
        Task<ApplicationUserToken> GetUserTokenByValueAsync(TUser user, string loginProvider, string tokenName);
        Task RemoveAuthenticationTokenByValueAsync(long userId, string value);
        Task<IdentityResult> AddLoginAsync(TUser user, UserLoginInfo login);
        Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword);
        Task<IdentityResult> ConfirmEmailAsync(TUser user, string token);
        Task<TUser> FindByEmailAsync(string email);
        Task<TUser> FindByLoginAsync(string loginProvider, string providerKey);
        Task<string> GenerateEmailConfirmationTokenAsync(TUser user);
        Task<string> GeneratePasswordResetTokenAsync(TUser user);
        Task<IdentityResult> RemoveLoginAsync(TUser user, string loginProvider, string providerKey);
        Task<TUser> GetUserAsync(ClaimsPrincipal principal);
        Task<bool> IsEmailConfirmedAsync(TUser user);
        Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword);
        Task<IdentityResult> UpdateAsync(TUser user);
        Task<string> EncryptUserIdAsync(long userId);
        Task<long> DecryptUserIdAsync(string userIdentityId);
        string NewSecurityStamp();
        Task<bool> HasPolicyAsync(TUser user, string policy);
        Task<bool> HasPolicyAsync(ClaimsPrincipal user, string policy);
        Task<TUser> FindByIdentityIdAsync(string userIdentityId);
        Task<TUser> FindByIdAsync(long userId);
    }
}
