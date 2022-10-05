using AutoMapper;
using Camino.Application.Contracts.AppServices.Authorization;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Camino.Infrastructure.Identity.Stores
{
    public class ApplicationRoleStore<TRole> : RoleStoreBase<TRole, long, ApplicationUserRole, ApplicationRoleClaim>, IApplicationRoleStore<TRole>
        where TRole : IdentityRole<long>
    {
        private readonly IRoleAppService _roleAppService;
        private readonly IRoleClaimAppService _roleClaimAppService;
        private readonly IMapper _mapper;

        public ApplicationRoleStore(IdentityErrorDescriber describer, IRoleAppService roleAppService,
            IRoleClaimAppService roleClaimAppService, IMapper mapper) : base(describer)
        {
            _roleAppService = roleAppService;
            _roleClaimAppService = roleClaimAppService;
            _mapper = mapper;
        }

        public override IQueryable<TRole> Roles { get; }

        public override async Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            var roleClaim = CreateRoleClaim(role, claim);
            var roleClaimRequest = new RoleClaimRequest
            {
                ClaimType = roleClaim.ClaimType,
                ClaimValue = roleClaim.ClaimValue,
                RoleId = roleClaim.RoleId
            };
            await _roleClaimAppService.CreateAsync(roleClaimRequest);
        }

        public override async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var applicationRole = _mapper.Map<RoleModifyRequest>(role);
            await _roleAppService.CreateAsync(applicationRole);
            return IdentityResult.Success;
        }

        public override async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            try
            {
                await _roleAppService.DeleteAsync(role.Id);
            }
            catch (Exception)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        public override async Task<TRole> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var roleId = ConvertIdFromString(id);
            var role = await _roleAppService.FindAsync(roleId);

            return _mapper.Map<TRole>(role);
        }

        public override async Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var role = await _roleAppService.FindByNameAsync(normalizedName);

            return _mapper.Map<TRole>(role);
        }

        public override async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var roleClaims = await _roleClaimAppService.GetByRoleIdAsync(role.Id);
            return roleClaims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
        }

        public override async Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            var claims = await _roleClaimAppService.GetByClaimAsync(role.Id, claim.Value, claim.Type);
            foreach (var c in claims)
            {
                await _roleClaimAppService.RemoveAsync(new RoleClaimRequest
                {
                    ClaimType = c.ClaimType,
                    ClaimValue = c.ClaimValue,
                    Id = c.Id,
                    RoleId = c.RoleId
                });
            }
        }

        public override async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            try
            {
                var roleRequest = _mapper.Map<RoleModifyRequest>(role);
                await _roleAppService.UpdateAsync(roleRequest);
            }
            // Todo: check DbUpdateConcurrencyException
            catch (Exception)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }
    }
}
