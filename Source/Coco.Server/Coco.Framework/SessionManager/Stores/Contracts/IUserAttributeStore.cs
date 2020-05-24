using Coco.Framework.Models;
using Coco.Entities.Dtos.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Stores.Contracts
{
    public interface IUserAttributeStore<TUser> where TUser : class
    {
        Task<string> GetSecurityStampAsync(long userId, string key);
        Task<string> GetSecurityStampAsync(long userId);
        ApplicationUserToken GetAuthenticationAttribute(long userId, string tokenName);
        Task<UserAttributeDto> SetResetPasswordStampAsync(ApplicationUser user, string stamp);
        Task<IEnumerable<UserAttributeDto>> SetAttributesAsync(IEnumerable<UserAttributeDto> userAttributes);
        IEnumerable<UserAttributeDto> NewUserRegisterAttributes(ApplicationUser user);
        IEnumerable<UserAttributeDto> NewUserAuthenticationAttributes(ApplicationUser user);
        Task<bool> DeleteUserAuthenticationAttributes(ApplicationUser user, string authenticationToken);
        Task<bool> DeleteUserAttribute(ApplicationUser user, string key);
        Task<bool> DeleteUserActivationAttribute(ApplicationUser user);
        Task<bool> DeleteResetPasswordByEmailAttribute(ApplicationUser user);
        string NewSecurityStamp();
    }
}
