using AutoMapper;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.Auth;
using Coco.Framework.Models;
using Coco.Framework.SessionManager.Stores.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Stores
{
    public class ApplicationRoleStore : RoleStoreBase<ApplicationRole, long, ApplicationUserRole, ApplicationRoleClaim>, IApplicationRoleStore<ApplicationRole>
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

        public override IQueryable<ApplicationRole> Roles { get; }

        public override Task AddClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default)
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
            var roleClaimDto = _mapper.Map<RoleClaimDto>(roleClaim);
            _roleClaimBusiness.Add(roleClaimDto);
            return Task.FromResult(false);
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var applicationRole = _mapper.Map<RoleDto>(role);
            await _roleBusiness.AddAsync(applicationRole);
            return IdentityResult.Success;
        }

        public override async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            try
            {
                _roleBusiness.Delete(role.Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        public override async Task<ApplicationRole> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var roleId = ConvertIdFromString(id);
            var role = await _roleBusiness.FindAsync(roleId);

            return _mapper.Map<ApplicationRole>(role);
        }

        public override async Task<ApplicationRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var role = await _roleBusiness.FindByNameAsync(normalizedName);

            return _mapper.Map<ApplicationRole>(role);
        }

        public override async Task<IList<Claim>> GetClaimsAsync(ApplicationRole role, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var roleClaims = await _roleClaimBusiness.GetByRoleIdAsync(role.Id);
            return roleClaims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
        }

        public override async Task RemoveClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default)
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

        public override async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            try
            {
                var roleDto = _mapper.Map<RoleDto>(role);
                await _roleBusiness.UpdateAsync(roleDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }
    }
}
