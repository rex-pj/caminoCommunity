using Coco.Api.Framework.Models;
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
        Task<ApiResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default);
        Task<TUser> FindByHashedIdAsync(string userIdHased, CancellationToken cancellationToken);
        Task<TUser> GetFullByFindByHashedIdAsync(string userIdHased, CancellationToken cancellationToken);
        Task<ApiResult> UpdateInfoAsync(TUser user, CancellationToken cancellationToken = default);
        Task<ApiResult> UpdateInfoItemAsync(UpdatePerItemModel user, CancellationToken cancellationToken = default);

    }
}
