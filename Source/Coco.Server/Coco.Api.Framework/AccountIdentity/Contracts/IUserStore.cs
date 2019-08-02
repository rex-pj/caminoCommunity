﻿using Coco.Api.Framework.Models;
using Coco.Entities.Enums;
using Coco.Entities.Model.Account;
using Coco.Entities.Model.General;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IUserStore<TUser> : IDisposable where TUser : class
    {
        Task<ApiResult> CreateAsync(TUser user, CancellationToken cancellationToken);
        Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken);
        Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);
        Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);
        Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken);
        Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken);
        Task<ApiResult> UpdateAuthenticationAsync(TUser user, CancellationToken cancellationToken = default);
        TUser FindByIdentityId(string userIdentityId, CancellationToken cancellationToken);
        Task<UserFullModel> GetFullByIdAsync(long id, CancellationToken cancellationToken);
        Task<UserFullModel> GetFullByFindByHashedIdAsync(string userIdentityId, CancellationToken cancellationToken);
        Task<ApiResult> UpdateInfoItemAsync(UpdatePerItemModel user, CancellationToken cancellationToken = default);
    }
}
