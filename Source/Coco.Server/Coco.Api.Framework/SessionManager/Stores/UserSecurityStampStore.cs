using Coco.Api.Framework.SessionManager.Contracts;
using Coco.Api.Framework.SessionManager.Core;
using Coco.Api.Framework.SessionManager.Entities;
using Coco.Business.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Api.Framework.SessionManager.Stores
{
    public class UserSecurityStampStore<TUser> : IUserSecurityStampStore<TUser> where TUser : IdentityUser<long>
    {
        private readonly IUserAttributeBusiness _userAttributeBusiness;
        public UserSecurityStampStore(IUserAttributeBusiness userAttributeBusiness)
        {
            _userAttributeBusiness = userAttributeBusiness;
        }

        public async Task<string> GetSecurityStampAsync(long userId, CancellationToken cancellationToken = default)
        {
            var data = await _userAttributeBusiness.GetAsync(userId, "");
            if (data != null)
            {
                return data.Value;
            }
            return null;
        }

        public async Task SetActiveUserStampAsync(long userId, string stamp, CancellationToken cancellationToken = default)
        {
           await _userAttributeBusiness.CreateOrUpdateAsync(userId, "", stamp);
        }

        public async Task SetResetPasswordStampAsync(long userId, string stamp, CancellationToken cancellationToken = default)
        {
            await _userAttributeBusiness.CreateOrUpdateAsync(userId, "", stamp);
        }

        public async Task SetPasswordSaltAsync(long userId, string stamp, CancellationToken cancellationToken = default)
        {
            await _userAttributeBusiness.CreateOrUpdateAsync(userId, UserAttributeOptions.SECURITY_SALT, stamp);
        }
    }
}
