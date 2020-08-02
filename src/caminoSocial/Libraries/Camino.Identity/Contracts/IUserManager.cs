using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Camino.IdentityManager.Contracts
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
        Task<IdentityResult> AddLoginAsync(TUser user, UserLoginInfo login);
        Task<IdentityResult> AddPasswordAsync(TUser user, string password);
        Task<IdentityResult> AddToRoleAsync(TUser user, string role);
        Task<IdentityResult> AddToRolesAsync(TUser user, IEnumerable<string> roles);
        Task<IdentityResult> ChangeEmailAsync(TUser user, string newEmail, string token);
        Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword);
        Task<IdentityResult> ChangePhoneNumberAsync(TUser user, string phoneNumber, string token);
        Task<IdentityResult> ConfirmEmailAsync(TUser user, string token);
        Task<int> CountRecoveryCodesAsync(TUser user);
        Task<byte[]> CreateSecurityTokenAsync(TUser user);
        Task<TUser> FindByEmailAsync(string email);
        Task<TUser> FindByIdAsync(string userId);
        Task<TUser> FindByLoginAsync(string loginProvider, string providerKey);
        Task<string> GenerateChangeEmailTokenAsync(TUser user, string newEmail);
        Task<string> GenerateChangePhoneNumberTokenAsync(TUser user, string phoneNumber);
        Task<string> GenerateConcurrencyStampAsync(TUser user);
        Task<string> GenerateEmailConfirmationTokenAsync(TUser user);
        string GenerateNewAuthenticatorKey();
        Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(TUser user, int number);
        Task<string> GeneratePasswordResetTokenAsync(TUser user);
        Task<string> GenerateTwoFactorTokenAsync(TUser user, string tokenProvider);
        Task<int> GetAccessFailedCountAsync(TUser user);
        Task<string> GetAuthenticatorKeyAsync(TUser user);
        Task<IList<Claim>> GetClaimsAsync(TUser user);
        Task<string> GetEmailAsync(TUser user);
        Task<bool> GetLockoutEnabledAsync(TUser user);
        Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user);
        Task<IdentityResult> RemoveLoginAsync(TUser user, string loginProvider, string providerKey);
        Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user);
        Task<string> GetPhoneNumberAsync(TUser user);
        Task<IList<string>> GetRolesAsync(TUser user);
        Task<string> GetSecurityStampAsync(TUser user);
        Task<bool> GetTwoFactorEnabledAsync(TUser user);
        Task<TUser> GetUserAsync(ClaimsPrincipal principal);
        string GetUserId(ClaimsPrincipal principal);
        Task<string> GetUserIdAsync(TUser user);
        string GetUserName(ClaimsPrincipal principal);
        Task<string> GetUserNameAsync(TUser user);
        Task<IList<TUser>> GetUsersForClaimAsync(Claim claim);
        Task<IList<TUser>> GetUsersInRoleAsync(string roleName);
        Task<IList<string>> GetValidTwoFactorProvidersAsync(TUser user);
        Task<bool> HasPasswordAsync(TUser user);
        Task<bool> IsEmailConfirmedAsync(TUser user);
        Task<bool> IsInRoleAsync(TUser user, string role);
        Task<string> EncryptUserIdAsync(long userId);
        Task<long> DecryptUserIdAsync(string userIdentityId);
        Task<bool> VerifyUserTokenAsync(TUser user, string tokenProvider, string purpose, string token);
        Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword);
        Task<IdentityResult> UpdateAsync(TUser user);
        string NewSecurityStamp();
        Task<bool> HasPolicyAsync(TUser user, string policy);
    }
}
