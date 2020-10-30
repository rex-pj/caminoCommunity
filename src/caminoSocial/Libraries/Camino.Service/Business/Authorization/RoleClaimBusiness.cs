using AutoMapper;
using Camino.Data.Contracts;
using Camino.Service.Projections.Identity;
using LinqToDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Authorization.Contracts;
using Camino.IdentityDAL.Entities;

namespace Camino.Service.Business.Authorization
{
    public class RoleClaimBusiness : IRoleClaimBusiness
    {
        private readonly IRepository<RoleClaim> _roleClaimRepository;
        private readonly IMapper _mapper;

        public RoleClaimBusiness(IRepository<RoleClaim> roleClaimRepository, IMapper mapper)
        {
            _roleClaimRepository = roleClaimRepository;
            _mapper = mapper;
        }

        public void Create(RoleClaimProjection RoleClaim)
        {
            var claim = _mapper.Map<RoleClaim>(RoleClaim);
            _roleClaimRepository.Add(claim);
        }

        public async Task<IList<RoleClaimProjection>> GetByRoleIdAsync(long roleId)
        {
            var RoleClaims = await _roleClaimRepository.GetAsync(x => x.RoleId == roleId);
            return _mapper.Map<IList<RoleClaimProjection>>(RoleClaims);
        }

        public async Task<IList<RoleClaimProjection>> GetByClaimAsync(long roleId, string claimValue, string claimType)
        {
            var RoleClaims = await _roleClaimRepository
                .GetAsync(x => x.RoleId == roleId && x.ClaimValue == claimValue && x.ClaimType == claimType);
            return _mapper.Map<IList<RoleClaimProjection>>(RoleClaims);
        }

        public void Remove(RoleClaimProjection RoleClaim)
        {
            var claim = _mapper.Map<RoleClaim>(RoleClaim);
            _roleClaimRepository.Delete(claim);
        }

        public async Task ReplaceClaimAsync(long roleId, ClaimProjection claim, ClaimProjection newClaim)
        {
            var matchedClaims = await GetByClaimAsync(roleId, claim.Value, claim.Type);
            foreach (var matchedClaim in matchedClaims)
            {
                matchedClaim.ClaimValue = newClaim.Value;
                matchedClaim.ClaimType = newClaim.Type;
            }
        }

        public async Task<IList<RoleProjection>> GetRolesForClaimAsync(ClaimProjection claim)
        {
            var existRoleClaims = await _roleClaimRepository.Get(x => x.ClaimValue == claim.Value && x.ClaimType == claim.Type)
                .Select(x => new RoleProjection()
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
