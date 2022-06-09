using Camino.Infrastructure.Identity.Core;

namespace Camino.Infrastructure.Identity.Interfaces
{
    public interface IUserPolicyStore<TUser> where TUser : ApplicationUser
    {
        Task<bool> HasPolicyAsync(TUser user, string policyName, CancellationToken cancellationToken = default);
    }
}
