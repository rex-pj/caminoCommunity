using Camino.Application.Contracts.AppServices.Authorization.Dtos;

namespace Camino.Application.Contracts.AppServices.Authorization
{
    public interface IRoleClaimAppService
    {
        Task<int> CreateAsync(RoleClaimRequest request);
        Task<IList<RoleClaimResult>> GetByClaimAsync(long roleId, string claimValue, string claimType);
        Task<IList<RoleClaimResult>> GetByRoleIdAsync(long roleId);
        Task<IList<RoleResult>> GetRolesForClaimAsync(ClaimRequest request);
        Task<bool> RemoveAsync(RoleClaimRequest request);
        Task ReplaceClaimAsync(long roleId, ClaimRequest claim, ClaimRequest newClaim);
    }
}