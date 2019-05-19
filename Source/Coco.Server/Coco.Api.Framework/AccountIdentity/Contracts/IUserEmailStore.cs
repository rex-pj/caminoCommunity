using Coco.Api.Framework.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IUserEmailStore<TUser> where TUser : class
    {
        Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken);
    }
}
