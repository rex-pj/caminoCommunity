using Camino.Shared.Requests.Authorization;
using Camino.Shared.Results.Authorization;
using Camino.Shared.Results.Identifiers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Authentication
{
    public interface IUserClaimRepository
    {
        void Create(UserClaimRequest userClaim);
        Task<IList<UserClaimResult>> GetByUserIdAsync(long userId);
        Task<IList<UserClaimResult>> GetByClaimAsync(long userId, string claimValue, string claimType);
        void Remove(UserClaimRequest userClaim);
        Task ReplaceClaimAsync(long userId, ClaimRequest claim, ClaimRequest newClaim);
        Task<IList<UserResult>> GetUsersByClaimAsync(ClaimRequest claim);
    }
}
