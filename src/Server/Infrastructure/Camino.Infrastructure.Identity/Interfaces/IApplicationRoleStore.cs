using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Camino.Infrastructure.Identity.Interfaces
{
    public interface IApplicationRoleStore<TRole> where TRole : IdentityRole<long>
    {
        Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default);
        Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default);
        Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default);
        Task<TRole> FindByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default);
        Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default);
        Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default);
        Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default);
    }
}
