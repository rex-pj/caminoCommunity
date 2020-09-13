using Camino.Service.Projections.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authorization.Contracts
{
    public interface IRoleClaimBusiness
    {
        void Add(RoleClaimProjection RoleClaim);
        Task<IList<RoleClaimProjection>> GetByClaimAsync(long roleId, string claimValue, string claimType);
        Task<IList<RoleClaimProjection>> GetByRoleIdAsync(long roleId);
        Task<IList<RoleProjection>> GetRolesForClaimAsync(ClaimProjection claim);
        void Remove(RoleClaimProjection RoleClaim);
        Task ReplaceClaimAsync(long roleId, ClaimProjection claim, ClaimProjection newClaim);
    }
}