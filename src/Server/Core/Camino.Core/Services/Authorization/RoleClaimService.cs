using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Core.Contracts.Services.Authorization;
using Camino.Shared.Requests.Authorization;
using Camino.Shared.Results.Authorization;
using Camino.Core.Contracts.Repositories.Authorization;

namespace Camino.Services.Authorization
{
    public class RoleClaimService : IRoleClaimService
    {
        private readonly IRoleClaimRepository _roleClaimRepository;

        public RoleClaimService(IRoleClaimRepository roleClaimRepository)
        {
            _roleClaimRepository = roleClaimRepository;
        }

        public void Create(RoleClaimRequest request)
        {
            _roleClaimRepository.Create(request);
        }

        public async Task<IList<RoleClaimResult>> GetByRoleIdAsync(long roleId)
        {
            return await _roleClaimRepository.GetByRoleIdAsync(roleId);
        }

        public async Task<IList<RoleClaimResult>> GetByClaimAsync(long roleId, string claimValue, string claimType)
        {
            return await _roleClaimRepository.GetByClaimAsync(roleId, claimValue, claimType);
        }

        public void Remove(RoleClaimRequest request)
        {
            _roleClaimRepository.Remove(request);
        }

        public async Task ReplaceClaimAsync(long roleId, ClaimRequest claim, ClaimRequest newClaim)
        {
            await _roleClaimRepository.ReplaceClaimAsync(roleId, claim, newClaim);
        }

        public async Task<IList<RoleResult>> GetRolesForClaimAsync(ClaimRequest request)
        {
            var existRoleClaims = await _roleClaimRepository.GetRolesForClaimAsync(request);
            return existRoleClaims;
        }
    }
}
