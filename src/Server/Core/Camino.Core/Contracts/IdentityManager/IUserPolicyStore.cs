using Camino.Core.Domain.Identities;
using System.Threading;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.IdentityManager
{
    public interface IUserPolicyStore<TUser> where TUser : ApplicationUser
    {
        Task<bool> HasPolicyAsync(TUser user, string policyName, CancellationToken cancellationToken = default);
    }
}
