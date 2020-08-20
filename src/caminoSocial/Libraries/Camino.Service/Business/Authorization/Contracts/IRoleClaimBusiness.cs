using Camino.Service.Data.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authorization.Contracts
{
    public interface IRoleClaimBusiness
    {
        void Add(RoleClaimDto RoleClaim);
        Task<IList<RoleClaimDto>> GetByClaimAsync(long roleId, string claimValue, string claimType);
        Task<IList<RoleClaimDto>> GetByRoleIdAsync(long roleId);
        Task<IList<RoleResult>> GetRolesForClaimAsync(ClaimResult claim);
        void Remove(RoleClaimDto RoleClaim);
        Task ReplaceClaimAsync(long roleId, ClaimResult claim, ClaimResult newClaim);
    }
}