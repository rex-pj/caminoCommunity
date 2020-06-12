using Coco.Framework.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Stores.Contracts
{
    public interface IApplicationRoleStore<TRole>
    {
        Task AddClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default);
        Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken = default);
        Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken = default);
        Task<ApplicationRole> FindByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<ApplicationRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default);
        Task<IList<Claim>> GetClaimsAsync(ApplicationRole role, CancellationToken cancellationToken = default);
        Task RemoveClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default);
        Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken = default);
    }
}
