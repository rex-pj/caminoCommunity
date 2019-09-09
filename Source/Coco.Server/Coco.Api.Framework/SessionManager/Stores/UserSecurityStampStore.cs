using Coco.Api.Framework.SessionManager.Contracts;
using Coco.Api.Framework.SessionManager.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Api.Framework.SessionManager.Stores
{
    public class UserSecurityStampStore<TUser> : IUserSecurityStampStore<TUser> where TUser : IdentityUser<long>
    {
        public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken = default)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task SetIdentityStampAsync(TUser user, string stamp, CancellationToken cancellationToken = default)
        {
            user.IdentityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task SetPasswordSaltAsync(TUser user, string stamp, CancellationToken cancellationToken = default)
        {
            user.PasswordSalt = stamp;
            return Task.FromResult(0);
        }
    }
}
