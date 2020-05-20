using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface IUserManager<TUser> : IDisposable where TUser : class
    {
        Task<IdentityResult> CreateAsync(TUser user);
        //string GetUserNameAsync(TUser user);
        //Task<string> GetEmailAsync(TUser user);
        //Task<TUser> FindByEmailAsync(string email);
        //Task<TUser> FindByNameAsync(string userName);
        //Task<string> GetUserIdAsync(TUser user);
        //Task<ICommonResult> CheckPasswordAsync(TUser user, string password);
        //TUser GetLoggingUser(string userIdentityId, string authenticationToken);
        //Task<UserFullDto> FindUserByIdentityIdAsync(string userIdentityId, string authenticationToken = null);
        //Task<UpdatePerItemModel> UpdateInfoItemAsync(UpdatePerItemModel model, string userIdentityId, string token);
        //Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(ApplicationUser user, string userIdentityId, string token);
        //Task<UserTokenResult> ChangePasswordAsync(long userId, string currentPassword, string newPassword);
        //Task<UserTokenResult> ResetPasswordAsync(ResetPasswordModel model, long userId);
        //Task<ICommonResult> ForgotPasswordAsync(string email);
        //Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
        //Task<bool> ClearUserLoginAsync(string userIdentityId, string authenticationToken);
        //Task<ICommonResult> ActiveAsync(string email, string activeKey);
        //Task<List<string>> GetRolesAsync(TUser user);
        //Task<ApplicationUserRoleAuthorizationPolicy> GetRoleAuthorizationsAsync(long id);
    }
}
