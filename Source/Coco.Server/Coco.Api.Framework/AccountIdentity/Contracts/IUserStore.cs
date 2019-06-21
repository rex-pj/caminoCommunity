using Coco.Api.Framework.AccountIdentity.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IUserStore<TUser> : IDisposable where TUser : class
    {
        Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken);
        Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken);
        Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);
        Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);
        Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken);
        Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken);
        Task<LoginResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default);
        Task<TUser> FindByHashedIdAsync(string userIdHased, CancellationToken cancellationToken);
        Task<TUser> GetFullByFindByHashedIdAsync(string userIdHased, CancellationToken cancellationToken);
        Task<IdentityResult> UpdateInfoAsync(TUser user, CancellationToken cancellationToken = default);

    }
}
