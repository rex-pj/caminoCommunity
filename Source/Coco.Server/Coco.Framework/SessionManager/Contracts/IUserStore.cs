using Coco.Framework.Models;
using Coco.Entities.Dtos.User;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface IUserStore<TUser> : IDisposable where TUser : class
    {
        Task<ICommonResult> CreateAsync(TUser user);
        string GetUserNameAsync(TUser user);
        Task<TUser> FindByEmailAsync(string normalizedEmail);
        Task<TUser> FindByNameAsync(string normalizedUserName);
        Task<TUser> FindByIdAsync(long userId);
        Task<string> GetUserIdAsync(TUser user);
        Task<ICommonResult> UpdateAuthenticationAsync(TUser user);
        TUser FindByIdentityId(string userIdentityId);
        Task<UserFullDto> FindFullByIdAsync(long id);
        Task<UserFullDto> FindByIdentityIdAsync(string userIdentityId);
        Task<UpdatePerItemModel> UpdateInfoItemAsync(UpdatePerItemModel user);
        Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(TUser user);
        Task<ICommonResult> ActiveAsync(TUser user);
        Task<List<ApplicationUserRole>> GetUserRolesAsync(TUser user);
        ApplicationUserRoleAuthorizationPolicy GetRoleAuthorizationsAsync(TUser user);
    }
}
