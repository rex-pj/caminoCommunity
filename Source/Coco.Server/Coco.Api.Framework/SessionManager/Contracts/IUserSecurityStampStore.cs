using System.Threading;
using System.Threading.Tasks;

namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IUserSecurityStampStore<TUser> where TUser : class
    {
        Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken = default);
        Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken);
        Task SetIdentityStampAsync(TUser user, string stamp, CancellationToken cancellationToken = default);
        Task SetPasswordSaltAsync(TUser user, string stamp, CancellationToken cancellationToken = default);
    }
}
