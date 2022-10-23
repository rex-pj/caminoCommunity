using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Core.Domains.Authentication.Repositories;
using Camino.Core.Domains.Authorization;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authentication
{
    public class UserClaimRepository : IUserClaimRepository, IScopedDependency
    {
        private readonly IEntityRepository<UserClaim> _userClaimRepository;
        private readonly IDbContext _dbContext;

        public UserClaimRepository(IEntityRepository<UserClaim> userClaimRepository, IDbContext dbContext)
        {
            _userClaimRepository = userClaimRepository;
            _dbContext = dbContext;
        }

        public async Task<int> CreateAsync(UserClaim userClaim)
        {
            _userClaimRepository.Insert(userClaim);
            await _dbContext.SaveChangesAsync();
            return userClaim.Id;
        }

        public async Task<IList<UserClaim>> GetByUserIdAsync(long userId)
        {
            var userClaims = await _userClaimRepository.GetAsync(x => x.UserId == userId);
            return userClaims;
        }

        public async Task<IList<UserClaim>> GetByClaimAsync(long userId, string claimValue, string claimType)
        {
            var userClaims = await _userClaimRepository
                .GetAsync(x => x.UserId == userId && x.ClaimValue == claimValue && x.ClaimType == claimType);
            return userClaims;
        }

        public async Task<bool> RemoveAsync(UserClaim userClaim)
        {
            _userClaimRepository.Delete(userClaim);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
