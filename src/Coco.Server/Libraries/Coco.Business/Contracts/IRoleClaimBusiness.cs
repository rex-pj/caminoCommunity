using Coco.Business.Dtos.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IRoleClaimBusiness
    {
        void Add(RoleClaimDto RoleClaim);
        Task<IList<RoleClaimDto>> GetByClaimAsync(long roleId, string claimValue, string claimType);
        Task<IList<RoleClaimDto>> GetByRoleIdAsync(long roleId);
        Task<IList<RoleDto>> GetRolesForClaimAsync(ClaimDto claim);
        void Remove(RoleClaimDto RoleClaim);
        Task ReplaceClaimAsync(long roleId, ClaimDto claim, ClaimDto newClaim);
    }
}