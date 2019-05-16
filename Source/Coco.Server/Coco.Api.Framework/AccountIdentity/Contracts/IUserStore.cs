using Coco.Api.Framework.AccountIdentity.Entities;
using Coco.Api.Framework.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IUserStore<TUser> where TUser : class
    {
        Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken);
        Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken);
        Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken);
    }
}
