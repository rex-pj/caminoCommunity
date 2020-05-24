using Coco.Entities.Dtos.Auth;
using Coco.Entities.Dtos.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IUserClaimBusiness
    {
        void Add(UserClaimDto userClaim);
        Task<IList<UserClaimDto>> GetByUserIdAsync(long userId);
        Task<IList<UserClaimDto>> GetByClaimAsync(long userId, string claimValue, string claimType);
        void Remove(UserClaimDto userClaim);
        Task ReplaceClaimAsync(long userId, ClaimDto claim, ClaimDto newClaim);
        Task<IList<UserDto>> GetUsersForClaimAsync(ClaimDto claim);
    }
}
