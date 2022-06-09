namespace Camino.Core.Domains.Authorization.Repositories
{
    public interface IRoleClaimRepository
    {
        Task<int> CreateAsync(RoleClaim roleClaim);
        Task<IList<RoleClaim>> GetByClaimAsync(long roleId, string claimValue, string claimType);
        Task<IList<RoleClaim>> GetByRoleIdAsync(long roleId);
        Task<bool> RemoveAsync(RoleClaim roleClaim);
        Task<IList<Role>> GetRolesByClaimAsync(string claimValue, string claimType);
    }
}
