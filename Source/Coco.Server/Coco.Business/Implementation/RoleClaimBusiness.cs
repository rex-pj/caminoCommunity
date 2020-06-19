using AutoMapper;
using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.Auth;
using LinqToDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation
{
    public class RoleClaimBusiness : IRoleClaimBusiness
    {
        private readonly IRepository<RoleClaim> _roleClaimRepository;
        //private readonly IdentityDbConnection _identityDbContext;
        private readonly IMapper _mapper;

        public RoleClaimBusiness(IRepository<RoleClaim> roleClaimRepository, IMapper mapper)
        {
            _roleClaimRepository = roleClaimRepository;
            //_identityDbContext = identityDbContext;
            _mapper = mapper;
        }

        public void Add(RoleClaimDto RoleClaim)
        {
            var claim = _mapper.Map<RoleClaim>(RoleClaim);
            _roleClaimRepository.Add(claim);
            //_identityDbContext.SaveChanges();
        }

        public async Task<IList<RoleClaimDto>> GetByRoleIdAsync(long roleId)
        {
            var RoleClaims = await _roleClaimRepository.GetAsync(x => x.RoleId == roleId);
            return _mapper.Map<IList<RoleClaimDto>>(RoleClaims);
        }

        public async Task<IList<RoleClaimDto>> GetByClaimAsync(long roleId, string claimValue, string claimType)
        {
            var RoleClaims = await _roleClaimRepository
                .GetAsync(x => x.RoleId == roleId && x.ClaimValue == claimValue && x.ClaimType == claimType);
            return _mapper.Map<IList<RoleClaimDto>>(RoleClaims);
        }

        public void Remove(RoleClaimDto RoleClaim)
        {
            var claim = _mapper.Map<RoleClaim>(RoleClaim);
            _roleClaimRepository.Delete(claim);
            //_identityDbContext.SaveChanges();
        }

        public async Task ReplaceClaimAsync(long roleId, ClaimDto claim, ClaimDto newClaim)
        {
            var matchedClaims = await GetByClaimAsync(roleId, claim.Value, claim.Type);
            foreach (var matchedClaim in matchedClaims)
            {
                matchedClaim.ClaimValue = newClaim.Value;
                matchedClaim.ClaimType = newClaim.Type;
            }
        }

        public async Task<IList<RoleDto>> GetRolesForClaimAsync(ClaimDto claim)
        {
            var existRoleClaims = await _roleClaimRepository.Get(x => x.ClaimValue == claim.Value && x.ClaimType == claim.Type)
                .Select(x => new RoleDto()
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
