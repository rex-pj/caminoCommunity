using Coco.Api.Framework.Models;
using Coco.Entities.Dtos.User;
using System;
using System.Threading.Tasks;

namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IUserStore<TUser> : IDisposable where TUser : class
    {
        Task<IApiResult> CreateAsync(TUser user);
        string GetUserNameAsync(TUser user);
        Task<TUser> FindByEmailAsync(string normalizedEmail);
        Task<TUser> FindByNameAsync(string normalizedUserName);
        Task<TUser> FindByIdAsync(long userId);
        Task<string> GetUserIdAsync(TUser user);
        Task<IApiResult> UpdateAuthenticationAsync(TUser user);
        TUser FindByIdentityId(string userIdentityId);
        Task<UserFullDto> FindFullByIdAsync(long id);
        Task<UserFullDto> FindByIdentityIdAsync(string userIdentityId);
        Task<UpdatePerItemModel> UpdateInfoItemAsync(UpdatePerItemModel user);
        Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(ApplicationUser user);
        Task<IApiResult> ActiveAsync(TUser user);
    }
}
