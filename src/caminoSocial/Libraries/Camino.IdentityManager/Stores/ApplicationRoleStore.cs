using AutoMapper;
using Camino.Service.Projections.Identity;
using Camino.IdentityManager.Models;
using Camino.IdentityManager.Contracts.Stores.Contracts;
using LinqToDB;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Camino.Service.Business.Authorization.Contracts;

namespace Camino.IdentityManager.Contracts.Stores
{
    public class ApplicationRoleStore<TRole> : RoleStoreBase<TRole, long, ApplicationUserRole, ApplicationRoleClaim>, IApplicationRoleStore<TRole>
        where TRole : IdentityRole<long>
    {
        private readonly IRoleBusiness _roleBusiness;
        private readonly IRoleClaimBusiness _roleClaimBusiness;
        private readonly IMapper _mapper;

        public ApplicationRoleStore(IdentityErrorDescriber describer, IRoleBusiness roleBusiness,
            IRoleClaimBusiness roleClaimBusiness, IMapper mapper) : base(describer)
        {
            _roleBusiness = roleBusiness;
            _roleClaimBusiness = roleClaimBusiness;
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
            var roleClaimRequest = _mapper.Map<RoleClaimProjection>(roleClaim);
            _roleClaimBusiness.Create(roleClaimRequest);
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

            var applicationRole = _mapper.Map<RoleProjection>(role);
            await _roleBusiness.AddAsync(applicationRole);
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
                await _roleBusiness.DeleteAsync(role.Id);
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
            var role = await _roleBusiness.FindAsync(roleId);

            return _mapper.Map<TRole>(role);
        }

        public override async Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var role = await _roleBusiness.FindByNameAsync(normalizedName);

            return _mapper.Map<TRole>(role);
        }

        public override async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var roleClaims = await _roleClaimBusiness.GetByRoleIdAsync(role.Id);
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
            var claims = await _roleClaimBusiness.GetByClaimAsync(role.Id, claim.Value, claim.Type);
            foreach (var c in claims)
            {
                _roleClaimBusiness.Remove(c);
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
                var roleRequest = _mapper.Map<RoleProjection>(role);
                await _roleBusiness.UpdateAsync(roleRequest);
            }
            // Todo: check DbUpdateConcurrencyException
            catch (LinqToDBException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }
    }
}
