using System.Threading;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IUserPasswordStore<TUser> where TUser : class
    {
        Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken = default);
    }
}
