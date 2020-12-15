using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Camino.IdentityManager.Contracts.Stores.Contracts
{
    public interface IUserPolicyStore<TUser> where TUser : IdentityUser<long>
    {
        Task<bool> HasPolicyAsync(TUser user, string policyName, CancellationToken cancellationToken = default);
    }
}
