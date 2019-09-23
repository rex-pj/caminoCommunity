using System.Threading;
using System.Threading.Tasks;

namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IUserSecurityStampStore<TUser> where TUser : class
    {
        Task<string> GetSecurityStampAsync(long userId, CancellationToken cancellationToken = default);
        Task SetSecurityStampAsync(long userId, string stamp, CancellationToken cancellationToken);
        Task SetIdentityStampAsync(long userId, string stamp, CancellationToken cancellationToken = default);
        Task SetPasswordSaltAsync(long userId, string stamp, CancellationToken cancellationToken = default);
    }
}
