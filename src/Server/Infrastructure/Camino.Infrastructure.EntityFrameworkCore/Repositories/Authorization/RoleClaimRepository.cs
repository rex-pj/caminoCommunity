using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Core.Domains;
using Camino.Core.Domains.Authorization;
using Camino.Core.Domains.Authorization.Repositories;
using Camino.Core.DependencyInjection;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authorization
{
    public class RoleClaimRepository : IRoleClaimRepository, IScopedDependency
    {
        private readonly IEntityRepository<RoleClaim> _roleClaimRepository;
        private readonly IDbContext _dbContext;

        public RoleClaimRepository(IEntityRepository<RoleClaim> roleClaimRepository, IDbContext dbContext)
        {
            _roleClaimRepository = roleClaimRepository;
            _dbContext = dbContext;
        }

        public async Task<int> CreateAsync(RoleClaim roleClaim)
        {
            _roleClaimRepository.Insert(roleClaim);
            _dbContext.SaveChanges();

            return roleClaim.Id;
        }

        public async Task<IList<RoleClaim>> GetByRoleIdAsync(long roleId)
        {
            var roleClaim = await _roleClaimRepository.GetAsync(x => x.RoleId == roleId);
            return roleClaim;
        }

        public async Task<IList<RoleClaim>> GetByClaimAsync(long roleId, string claimValue, string claimType)
        {
            var roleClaims = await _roleClaimRepository
                .GetAsync(x => x.RoleId == roleId && x.ClaimValue == claimValue && x.ClaimType == claimType);
            return roleClaims;
        }

        public async Task<IList<Role>> GetRolesByClaimAsync(string claimValue, string claimType)
        {
            var roleClaims = await _roleClaimRepository
                .GetAsync(x => x.ClaimValue == claimValue && x.ClaimType == claimType, x => x.Role);
            return roleClaims;
        }

        public async Task<bool> RemoveAsync(RoleClaim roleClaim)
        {
            _roleClaimRepository.Delete(roleClaim);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
