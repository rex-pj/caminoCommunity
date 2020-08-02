using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Camino.IdentityManager.Contracts
{
    public interface IApplicationRoleManager<TRole> where TRole : IdentityRole<long>
    {
        Task<IdentityResult> AddClaimAsync(TRole role, Claim claim);
        Task<IdentityResult> CreateAsync(TRole role);
        Task<IdentityResult> DeleteAsync(TRole role);
        Task<TRole> FindByIdAsync(string roleId);
        Task<TRole> FindByNameAsync(string roleName);
        Task<IList<Claim>> GetClaimsAsync(TRole role);
        Task<string> GetRoleIdAsync(TRole role);
        Task<string> GetRoleNameAsync(TRole role);
        string NormalizeKey(string key);
        Task<IdentityResult> RemoveClaimAsync(TRole role, Claim claim);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> SetRoleNameAsync(TRole role, string name);
        Task<IdentityResult> UpdateAsync(TRole role);
        Task UpdateNormalizedRoleNameAsync(TRole role);
    }
}
