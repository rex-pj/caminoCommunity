using Camino.Service.Data.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authentication.Contracts
{
    public interface IUserClaimBusiness
    {
        void Add(UserClaimResult userClaim);
        Task<IList<UserClaimResult>> GetByUserIdAsync(long userId);
        Task<IList<UserClaimResult>> GetByClaimAsync(long userId, string claimValue, string claimType);
        void Remove(UserClaimResult userClaim);
        Task ReplaceClaimAsync(long userId, ClaimResult claim, ClaimResult newClaim);
        Task<IList<UserResult>> GetUsersForClaimAsync(ClaimResult claim);
    }
}
