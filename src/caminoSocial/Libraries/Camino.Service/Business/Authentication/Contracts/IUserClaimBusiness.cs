using Camino.Service.Data.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authentication.Contracts
{
    public interface IUserClaimBusiness
    {
        void Add(UserClaimProjection userClaim);
        Task<IList<UserClaimProjection>> GetByUserIdAsync(long userId);
        Task<IList<UserClaimProjection>> GetByClaimAsync(long userId, string claimValue, string claimType);
        void Remove(UserClaimProjection userClaim);
        Task ReplaceClaimAsync(long userId, ClaimProjection claim, ClaimProjection newClaim);
        Task<IList<UserProjection>> GetUsersForClaimAsync(ClaimProjection claim);
    }
}
