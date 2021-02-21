using Camino.Shared.Requests.Authorization;
using Camino.Shared.Results.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Authorization
{
    public interface IRoleClaimRepository
    {
        void Create(RoleClaimRequest request);
        Task<IList<RoleClaimResult>> GetByClaimAsync(long roleId, string claimValue, string claimType);
        Task<IList<RoleClaimResult>> GetByRoleIdAsync(long roleId);
        Task<IList<RoleResult>> GetRolesForClaimAsync(ClaimRequest request);
        void Remove(RoleClaimRequest request);
        Task ReplaceClaimAsync(long roleId, ClaimRequest claim, ClaimRequest newClaim);
    }
}