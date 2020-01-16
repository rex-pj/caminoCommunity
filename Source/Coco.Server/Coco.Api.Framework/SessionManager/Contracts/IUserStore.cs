﻿using Coco.Api.Framework.Models;
using Coco.Entities.Dtos.User;
using System;
using System.Threading.Tasks;

namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IUserStore<TUser> : IDisposable where TUser : class
    {
        Task<ApiResult> CreateAsync(TUser user);
        string GetUserNameAsync(TUser user);
        Task<TUser> FindByEmailAsync(string normalizedEmail);
        Task<TUser> FindByNameAsync(string normalizedUserName);
        Task<TUser> FindByIdAsync(long userId);
        Task<string> GetUserIdAsync(TUser user);
        Task<ApiResult> UpdateAuthenticationAsync(TUser user);
        TUser FindByIdentityId(string userIdentityId);
        Task<UserFullDto> FindFullByIdAsync(long id);
        Task<UserFullDto> FindByIdentityIdAsync(string userIdentityId);
        Task<ApiResult> UpdateInfoItemAsync(UpdatePerItemModel user);
        Task<ApiResult> UpdateIdentifierAsync(ApplicationUser user);
        Task<ApiResult> ActiveAsync(TUser user);
    }
}