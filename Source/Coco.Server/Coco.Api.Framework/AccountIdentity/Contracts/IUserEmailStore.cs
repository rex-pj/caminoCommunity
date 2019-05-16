using Coco.Api.Framework.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IUserEmailStore<TUser> where TUser : class
    {
        Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken);
        Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken);
    }
}
