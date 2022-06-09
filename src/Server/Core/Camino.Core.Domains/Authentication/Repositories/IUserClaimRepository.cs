using Camino.Core.Domains.Authorization;

namespace Camino.Core.Domains.Authentication.Repositories
{
    public interface IUserClaimRepository
    {
        Task<int> CreateAsync(UserClaim userClaim);
        Task<IList<UserClaim>> GetByClaimAsync(long userId, string claimValue, string claimType);
        Task<IList<UserClaim>> GetByUserIdAsync(long userId);
        Task<bool> RemoveAsync(UserClaim userClaim);
    }
}
