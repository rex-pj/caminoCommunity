using Coco.Framework.Models;
using Coco.Entities.Dtos.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Stores.Contracts
{
    public interface IUserStampStore<TUser> where TUser : class
    {
        Task<string> GetSecurityStampAsync(long userId, string key);
        Task<string> GetPasswordSaltAsync(long userId);
        UserTokenResult GetAuthenticationAttribute(long userId, string authenticationToken);
        Task<UserAttributeDto> SetResetPasswordStampAsync(ApplicationUser user, string stamp);
        Task<IEnumerable<UserAttributeDto>> SetAttributesAsync(IEnumerable<UserAttributeDto> userAttributes);
        IEnumerable<UserAttributeDto> NewUserRegisterAttributes(ApplicationUser user);
        IEnumerable<UserAttributeDto> NewUserAuthenticationAttributes(ApplicationUser user);
        Task<bool> DeleteUserAuthenticationAttributes(ApplicationUser user, string authenticationToken);
        Task<string> GetActivationKeyAsync(long userId);
        Task<bool> DeleteUserAttribute(ApplicationUser user, string key);
        Task<bool> DeleteUserActivationAttribute(ApplicationUser user);
        Task<string> GetResetPasswordKeyAsync(long userId);
        Task<bool> DeleteResetPasswordByEmailAttribute(ApplicationUser user);
        string NewSecurityStamp();
        string NewSecuritySalt();
    }
}
