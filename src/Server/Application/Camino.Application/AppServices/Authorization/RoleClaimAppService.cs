using Camino.Application.Contracts.AppServices.Authorization;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Domains.Authorization;
using Camino.Core.Domains.Authorization.Repositories;
using Camino.Core.DependencyInjection;

namespace Camino.Application.AppServices.Authorization
{
    public class RoleClaimAppService : IRoleClaimAppService, IScopedDependency
    {
        private readonly IRoleClaimRepository _roleClaimRepository;
        private readonly IUserRepository _userRepository;

        public RoleClaimAppService(IRoleClaimRepository roleClaimRepository, IUserRepository userRepository)
        {
            _roleClaimRepository = roleClaimRepository;
            _userRepository = userRepository;
        }

        public async Task<int> CreateAsync(RoleClaimRequest request)
        {
            var claim = new RoleClaim
            {
                ClaimType = request.ClaimType,
                RoleId = request.RoleId,
                ClaimValue = request.ClaimValue
            };
            return await _roleClaimRepository.CreateAsync(claim);
        }

        public async Task<IList<RoleClaimResult>> GetByRoleIdAsync(long roleId)
        {
            var roleClaims = await _roleClaimRepository.GetByRoleIdAsync(roleId);
            if (roleClaims == null)
            {
                return new List<RoleClaimResult>();
            }

            return roleClaims.Select(x => MapEntityToDto(x)).ToList();
        }

        public async Task<IList<RoleClaimResult>> GetByClaimAsync(long roleId, string claimValue, string claimType)
        {
            var roleClaims = await _roleClaimRepository.GetByClaimAsync(roleId, claimValue, claimType);
            if (roleClaims == null)
            {
                return new List<RoleClaimResult>();
            }

            return roleClaims.Select(x => MapEntityToDto(x)).ToList();
        }

        private RoleClaimResult MapEntityToDto(RoleClaim x)
        {
            return new RoleClaimResult
            {
                ClaimType = x.ClaimType,
                ClaimValue = x.ClaimValue,
                RoleId = x.RoleId,
                Id = x.Id
            };
        }

        public async Task<bool> RemoveAsync(RoleClaimRequest request)
        {
            var roleClaim = new RoleClaim
            {
                ClaimType = request.ClaimType,
                RoleId = request.RoleId,
                ClaimValue = request.ClaimValue,
                Id = request.Id
            };
            return await _roleClaimRepository.RemoveAsync(roleClaim);
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

        public async Task<IList<RoleResult>> GetRolesForClaimAsync(ClaimRequest request)
        {
            var roles = await _roleClaimRepository.GetRolesByClaimAsync(request.Value, request.Type);
            if (roles == null)
            {
                return new List<RoleResult>();
            }

            var result = roles.Select(x => MapEntityToDto(x)).ToList();
            await PopulateDetailsAsync(result);

            return result;
        }

        private async Task PopulateDetailsAsync(IList<RoleResult> roles)
        {
            var createdByIds = roles.Select(x => x.CreatedById).ToArray();
            var updatedByIds = roles.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = await _userRepository.GetByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetByIdsAsync(updatedByIds);

            foreach (var role in roles)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == role.CreatedById);
                role.CreatedByName = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == role.UpdatedById);
                role.UpdatedByName = updatedBy.DisplayName;
            }
        }

        private RoleResult MapEntityToDto(Role x)
        {
            return new RoleResult
            {
                Id = x.Id,
                CreatedById = x.CreatedById,
                CreatedByName = x.CreatedBy.Lastname + " " + x.CreatedBy.Firstname,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                Name = x.Name,
                UpdatedById = x.UpdatedById,
                UpdatedByName = x.UpdatedBy.Lastname + " " + x.UpdatedBy.Firstname,
                UpdatedDate = x.UpdatedDate
            };
        }
    }
}
