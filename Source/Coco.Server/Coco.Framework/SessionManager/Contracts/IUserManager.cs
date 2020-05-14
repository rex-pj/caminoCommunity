using Coco.Framework.Models;
using Coco.Entities.Enums;
using Coco.Entities.Dtos.User;
using Coco.Entities.Dtos.General;
using System;
using System.Threading.Tasks;
using Coco.Framework.SessionManager.Core;
using System.Collections.Generic;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface IUserManager<TUser> : IDisposable where TUser : class
    {
        IdentityOptions Options { get; set; }
        Task<ICommonResult> CreateAsync(TUser user);
        string GetUserNameAsync(TUser user);
        Task<string> GetEmailAsync(TUser user);
        Task<TUser> FindByEmailAsync(string email);
        Task<TUser> FindByNameAsync(string userName);
        Task<string> GetUserIdAsync(TUser user);
        Task<ICommonResult> CheckPasswordAsync(TUser user, string password);
        TUser GetLoggingUser(string userIdentityId, string authenticationToken);
        Task<UserFullDto> FindUserByIdentityIdAsync(string userIdentityId, string authenticationToken = null);
        Task<UpdatePerItemModel> UpdateInfoItemAsync(UpdatePerItemModel model, string userIdentityId, string token);
        Task<UserPhotoUpdateDto> UpdateAvatarAsync(UserPhotoUpdateDto model, ApplicationUser user);
        Task<UserPhotoUpdateDto> UpdateCoverAsync(UserPhotoUpdateDto model, ApplicationUser user);
        Task<UserPhotoUpdateDto> DeleteUserPhotoAsync(long userId, UserPhotoTypeEnum userPhotoType);
        Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(ApplicationUser user, string userIdentityId, string token);
        Task<UserTokenResult> ChangePasswordAsync(long userId, string currentPassword, string newPassword);
        Task<UserTokenResult> ResetPasswordAsync(ResetPasswordModel model, long userId);
        Task<ICommonResult> ForgotPasswordAsync(string email);
        Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
        Task<bool> ClearUserLoginAsync(string userIdentityId, string authenticationToken);
        Task<ICommonResult> ActiveAsync(string email, string activeKey);
        Task<List<string>> GetRolesAsync(TUser user);
    }
}
