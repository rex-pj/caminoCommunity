using Coco.Api.Framework.Models;
using Coco.Entities.Enums;
using Coco.Entities.Dtos.User;
using Coco.Entities.Dtos.General;
using System;
using System.Threading.Tasks;
using Coco.Api.Framework.SessionManager.Core;

namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IUserManager<TUser> : IDisposable where TUser : class
    {
        IdentityOptions Options { get; set; }
        Task<ApiResult> CreateAsync(TUser user);
        string GetUserNameAsync(TUser user);
        Task<string> GetEmailAsync(TUser user);
        Task<TUser> FindByEmailAsync(string email);
        Task<TUser> FindByNameAsync(string userName);
        Task<string> GetUserIdAsync(TUser user);
        Task<ApiResult> CheckPasswordAsync(TUser user, string password);
        TUser GetLoggingUser(string userIdentityId, string authenticationToken);
        Task<UserFullDto> FindUserByIdentityIdAsync(string userIdentityId, string authenticationToken = null);
        Task<ApiResult> UpdateInfoItemAsync(UpdatePerItemModel model, string userIdentityId, string token);
        Task<ApiResult> UpdateAvatarAsync(UpdateUserPhotoDto model, long userId);
        Task<ApiResult> UpdateCoverAsync(UpdateUserPhotoDto model, long userId);
        Task<ApiResult> DeleteUserPhotoAsync(long userId, UserPhotoTypeEnum userPhotoType);
        Task<ApiResult> UpdateIdentifierAsync(ApplicationUser user, string userIdentityId, string token);
        Task<ApiResult> ChangePasswordAsync(long userId, string currentPassword, string newPassword);
        Task<ApiResult> ResetPasswordAsync(ResetPasswordModel model, long userId);
        Task<ApiResult> ForgotPasswordAsync(string email);
        Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
        Task<ApiResult> ClearUserLoginAsync(string userIdentityId, string authenticationToken);
        Task<ApiResult> ActiveAsync(string email, string activeKey);
    }
}
