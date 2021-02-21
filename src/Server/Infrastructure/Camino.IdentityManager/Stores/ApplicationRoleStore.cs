using AutoMapper;
using Camino.Core.Domain.Identities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Camino.Core.Contracts.Services.Authorization;
using Camino.Core.Contracts.IdentityManager;
using Camino.Shared.Requests.Authorization;

namespace Camino.IdentityManager.Contracts.Stores
{
    public class ApplicationRoleStore<TRole> : RoleStoreBase<TRole, long, ApplicationUserRole, ApplicationRoleClaim>, IApplicationRoleStore<TRole>
        where TRole : IdentityRole<long>
    {
        private readonly IRoleService _roleService;
        private readonly IRoleClaimService _roleClaimService;
        private readonly IMapper _mapper;

        public ApplicationRoleStore(IdentityErrorDescriber describer, IRoleService roleService,
            IRoleClaimService roleClaimService, IMapper mapper) : base(describer)
        {
            _roleService = roleService;
            _roleClaimService = roleClaimService;
            _mapper = mapper;
        }

        public override IQueryable<TRole> Roles { get; }

        public override Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
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
            _roleClaimService.Create(roleClaimRequest);
            return Task.FromResult(false);
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
            await _roleService.CreateAsync(applicationRole);
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
                await _roleService.DeleteAsync(role.Id);
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
            var role = await _roleService.FindAsync(roleId);

            return _mapper.Map<TRole>(role);
        }

        public override async Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var role = await _roleService.FindByNameAsync(normalizedName);

            return _mapper.Map<TRole>(role);
        }

        public override async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var roleClaims = await _roleClaimService.GetByRoleIdAsync(role.Id);
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
            var claims = await _roleClaimService.GetByClaimAsync(role.Id, claim.Value, claim.Type);
            foreach (var c in claims)
            {
                _roleClaimService.Remove(new RoleClaimRequest
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
                await _roleService.UpdateAsync(roleRequest);
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
