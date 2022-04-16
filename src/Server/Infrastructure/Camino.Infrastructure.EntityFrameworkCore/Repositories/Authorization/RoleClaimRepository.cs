using Camino.Core.Contracts.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Authorization;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Requests.Authorization;
using Camino.Shared.Results.Authorization;
using Camino.Core.Contracts.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authorization
{
    public class RoleClaimRepository : IRoleClaimRepository, IScopedDependency
    {
        private readonly IRepository<RoleClaim> _roleClaimRepository;
        private readonly IAppDbContext _dbContext;

        public RoleClaimRepository(IRepository<RoleClaim> roleClaimRepository, IAppDbContext dbContext)
        {
            _roleClaimRepository = roleClaimRepository;
            _dbContext = dbContext;
        }

        public void Create(RoleClaimRequest request)
        {
            var claim = new RoleClaim
            {
                ClaimType = request.ClaimType,
                RoleId = request.RoleId,
                ClaimValue = request.ClaimValue
            };
            _roleClaimRepository.Insert(claim);
            _dbContext.SaveChanges();
        }

        public async Task<IList<RoleClaimResult>> GetByRoleIdAsync(long roleId)
        {
            var roleClaims = await _roleClaimRepository.Get(x => x.RoleId == roleId)
                .Select(x => new RoleClaimResult
                {
                    ClaimType = x.ClaimType,
                    ClaimValue = x.ClaimValue,
                    RoleId = x.RoleId,
                    Id = x.Id
                }).ToListAsync();
            return roleClaims;
        }

        public async Task<IList<RoleClaimResult>> GetByClaimAsync(long roleId, string claimValue, string claimType)
        {
            var roleClaims = await _roleClaimRepository
                .Get(x => x.RoleId == roleId && x.ClaimValue == claimValue && x.ClaimType == claimType)
                .Select(x => new RoleClaimResult
                {
                    ClaimType = x.ClaimType,
                    ClaimValue = x.ClaimValue,
                    RoleId = x.RoleId,
                    Id = x.Id
                }).ToListAsync();
            return roleClaims;
        }

        public void Remove(RoleClaimRequest request)
        {
            var claim = new RoleClaim
            {
                ClaimType = request.ClaimType,
                RoleId = request.RoleId,
                ClaimValue = request.ClaimValue,
                Id = request.Id
            };
            _roleClaimRepository.Delete(claim);
            _dbContext.SaveChanges();
        }

        public async Task ReplaceClaimAsync(long roleId, ClaimRequest claim, ClaimRequest newClaim)
        {
            var matchedClaims = await GetByClaimAsync(roleId, claim.Value, claim.Type);
            foreach (var matchedClaim in matchedClaims)
            {
                matchedClaim.ClaimValue = newClaim.Value;
                matchedClaim.ClaimType = newClaim.Type;
            }
        }

        public async Task<IList<RoleResult>> GetRolesForClaimAsync(ClaimRequest claim)
        {
            var existRoleClaims = await _roleClaimRepository.Get(x => x.ClaimValue == claim.Value && x.ClaimType == claim.Type)
                .Select(x => new RoleResult()
                {
                    Id = x.RoleId,
                    CreatedById = x.Role.CreatedById,
                    CreatedByName = x.Role.CreatedBy.Lastname + " " + x.Role.CreatedBy.Firstname,
                    CreatedDate = x.Role.CreatedDate,
                    Description = x.Role.Description,
                    Name = x.Role.Name,
                    UpdatedById = x.Role.UpdatedById,
                    UpdatedByName = x.Role.UpdatedBy.Lastname + " " + x.Role.UpdatedBy.Firstname,
                    UpdatedDate = x.Role.UpdatedDate
                }).ToListAsync();

            return existRoleClaims;
        }
    }
}
