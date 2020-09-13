using AutoMapper;
using Camino.Service.Projections.Identity;
using Camino.IdentityManager.Models;
using Camino.IdentityManager.Contracts.Stores.Contracts;
using LinqToDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Camino.IdentityManager.Contracts.Core;
using Camino.Service.Business.Users.Contracts;
using Camino.Service.Business.Authentication.Contracts;
using Camino.Service.Business.Authorization.Contracts;
using Camino.Service.Projections.Request;

namespace Camino.IdentityManager.Contracts.Stores
{
    public class ApplicationUserStore<TUser> : UserStoreBase<TUser, ApplicationRole, long, ApplicationUserClaim,
        ApplicationUserRole, ApplicationUserLogin, ApplicationUserToken, ApplicationRoleClaim>,
        IUserPasswordStore<TUser>, IUserAuthenticationTokenStore<TUser>,
        IUserEncryptionStore<TUser>, IUserSecurityStampStore<TUser>,
        IUserPolicyStore<TUser>
        where TUser : IdentityUser<long>
    {
        private readonly IMapper _mapper;

        private readonly IUserBusiness _userBusiness;
        private readonly IUserClaimBusiness _userClaimBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IRoleBusiness _roleBusiness;
        private readonly IUserTokenBusiness _userTokenBusiness;
        private readonly IUserLoginBusiness _userLoginBusiness;
        private readonly IAuthorizationPolicyBusiness _authorizationPolicyBusiness;
        private readonly IUserAuthorizationPolicyBusiness _userAuthorizationPolicyBusiness;
        private readonly ITextEncryption _textCrypter;
        private readonly CrypterSettings _crypterSettings;

        public override IQueryable<TUser> Users { get; }

        public ApplicationUserStore(IdentityErrorDescriber describer, IUserBusiness userBusiness,
            IUserClaimBusiness userClaimBusiness, IUserRoleBusiness userRoleBusiness, IRoleBusiness roleBusiness,
            IUserTokenBusiness userTokenBusiness, IUserLoginBusiness userLoginBusiness, IAuthorizationPolicyBusiness authorizationPolicyBusiness,
            IUserAuthorizationPolicyBusiness userAuthorizationPolicyBusiness, ITextEncryption textCrypter, 
            IMapper mapper, IOptions<CrypterSettings> crypterSettings)
            : base(describer)
        {
            _crypterSettings = crypterSettings.Value;
            _userBusiness = userBusiness;
            _userClaimBusiness = userClaimBusiness;
            _userRoleBusiness = userRoleBusiness;
            _userTokenBusiness = userTokenBusiness;
            _roleBusiness = roleBusiness;
            _userLoginBusiness = userLoginBusiness;
            _authorizationPolicyBusiness = authorizationPolicyBusiness;
            _userAuthorizationPolicyBusiness = userAuthorizationPolicyBusiness;
            _mapper = mapper;
            _textCrypter = textCrypter;
        }

        public override async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }

                var userRequest = _mapper.Map<UserProjection>(user);
                userRequest.PasswordHash = user.PasswordHash;

                await _userBusiness.CreateAsync(userRequest);

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = ex.Message,
                    Description = ex.ToString()
                });
            }
        }

        public override async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var id = long.Parse(userId);
            var user = await _userBusiness.FindByIdAsync(id);
            return _mapper.Map<TUser>(user);
        }

        public override async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            try
            {
                await _userBusiness.DeleteAsync(user.Id);
            }
            // Todo: check DbUpdateConcurrencyException
            catch (LinqToDBException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        public override async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            var user = await _userBusiness.FindByUsernameAsync(normalizedUserName);
            return _mapper.Map<TUser>(user);
        }

        public override async Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken = default)
        {
            return await base.GetSecurityStampAsync(user, cancellationToken);
        }

        public override async Task AddToRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(nameof(normalizedRoleName));
            }
            var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (roleEntity == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Role {0} does not exist.", normalizedRoleName));
            }
            var applicationUserRole = CreateUserRole(user, roleEntity);
            var userRoleRequest = _mapper.Map<UserRoleProjection>(applicationUserRole);
            _userRoleBusiness.Add(userRoleRequest);
        }

        protected override async Task<ApplicationRole> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var role = await _roleBusiness.FindByNameAsync(normalizedRoleName);
            return _mapper.Map<ApplicationRole>(role);
        }

        protected override async Task<ApplicationUserRole> FindUserRoleAsync(long userId, long roleId, CancellationToken cancellationToken)
        {
            var userRole = await _userRoleBusiness.FindUserRoleAsync(userId, roleId);
            return _mapper.Map<ApplicationUserRole>(userRole);
        }

        public override async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var roles = (await _userRoleBusiness.GetUserRolesAsync(user.Id))
                .Select(x => x.RoleName).ToList();

            return roles;
        }

        public override async Task<IList<TUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(normalizedRoleName))
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }

            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);

            if (role != null)
            {
                var users = await _userRoleBusiness.GetUsersInRoleAsync(role.Id);
                return _mapper.Map<IList<TUser>>(users);
            }
            return new List<TUser>();
        }

        public override async Task<bool> IsInRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(nameof(normalizedRoleName));
            }
            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (role != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
                return userRole != null;
            }
            return false;
        }

        public override async Task RemoveFromRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(nameof(normalizedRoleName));
            }

            var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (roleEntity != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, roleEntity.Id, cancellationToken);
                if (userRole != null)
                {
                    var userRoleRequest = _mapper.Map<UserRoleProjection>(userRole);
                    _userRoleBusiness.Remove(userRoleRequest);
                }
            }
        }

        public override Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }
            foreach (var claim in claims)
            {
                var userClaim = _mapper.Map<UserClaimProjection>(CreateUserClaim(user, claim));
                _userClaimBusiness.Add(userClaim);
            }
            return Task.FromResult(false);
        }

        protected override ApplicationUserClaim CreateUserClaim(TUser user, Claim claim)
        {
            var userClaim = new ApplicationUserClaim { UserId = user.Id };
            userClaim.InitializeFromClaim(claim);
            return userClaim;
        }

        public override async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            var user = await _userBusiness.FindByEmailAsync(normalizedEmail);
            return _mapper.Map<TUser>(user);
        }

        protected override async Task<TUser> FindUserAsync(long userId, CancellationToken cancellationToken)
        {
            var user = await _userBusiness.FindByIdAsync(userId);
            return _mapper.Map<TUser>(user);
        }

        public override Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            var userLogin = CreateUserLogin(user, login);
            var userLoginRequest = _mapper.Map<UserLoginRequest>(userLogin);
            _userLoginBusiness.Add(userLoginRequest);
            return Task.FromResult(false);
        }

        protected override async Task<ApplicationUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var userLoginRequest = await _userLoginBusiness.FindAsync(loginProvider, providerKey);
            return _mapper.Map<ApplicationUserLogin>(userLoginRequest);
        }

        protected override async Task<ApplicationUserLogin> FindUserLoginAsync(long userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var userLoginRequest = await _userLoginBusiness.FindAsync(userId, loginProvider, providerKey);
            return _mapper.Map<ApplicationUserLogin>(userLoginRequest);
        }

        public override async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            var userInfos = (await _userLoginBusiness.GetByUserIdAsync(user.Id))
                .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName))
                .ToList();

            return userInfos;
        }

        public override async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var entry = await FindUserLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);
            if (entry != null)
            {
                var userLogin = _mapper.Map<UserLoginRequest>(entry);
                _userLoginBusiness.Remove(userLogin);
            }
        }

        public override async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userClaims = await _userClaimBusiness.GetByUserIdAsync(user.Id);
            var claims = _mapper.Map<IList<ApplicationUserClaim>>(userClaims);
            return claims.Select(c => c.ToClaim()).ToList();
        }

        public override async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            var claimRequest = _mapper.Map<ClaimProjection>(claim);
            var userClaims = await _userClaimBusiness.GetUsersForClaimAsync(claimRequest);
            var applicationUserClaims = _mapper.Map<IList<TUser>>(userClaims);

            return applicationUserClaims;
        }

        public override async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            foreach (var claim in claims)
            {
                var userClaims = await _userClaimBusiness.GetByClaimAsync(user.Id, claim.Value, claim.Type);
                foreach (var c in userClaims)
                {
                    _userClaimBusiness.Remove(c);
                }
            }
        }

        protected override Task AddUserTokenAsync(ApplicationUserToken token)
        {
            var userToken = _mapper.Map<UserTokenProjection>(token);
            _userTokenBusiness.Add(userToken);
            return Task.CompletedTask;
        }

        protected override async Task<ApplicationUserToken> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            var userToken = await _userTokenBusiness.FindAsync(user.Id, loginProvider, name);
            return _mapper.Map<ApplicationUserToken>(userToken);
        }

        protected override Task RemoveUserTokenAsync(ApplicationUserToken token)
        {
            var userToken = _mapper.Map<UserTokenProjection>(token);
            _userTokenBusiness.Remove(userToken);
            return Task.CompletedTask;
        }

        public override async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            if (newClaim == null)
            {
                throw new ArgumentNullException(nameof(newClaim));
            }

            var claimRequest = _mapper.Map<ClaimProjection>(claim);
            var newClaimRequest = _mapper.Map<ClaimProjection>(newClaim);
            await _userClaimBusiness.ReplaceClaimAsync(user.Id, claimRequest, newClaimRequest);
        }

        public override async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.ConcurrencyStamp = Guid.NewGuid().ToString();

            try
            {
                var userRequest = _mapper.Map<UserProjection>(user);
                await _userBusiness.UpdateAsync(userRequest);
            }
            // todo: check DbUpdateConcurrencyException
            catch (LinqToDBException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        public async Task<string> EncryptUserId(long userId)
        {
            var encryptId = _textCrypter.Encrypt(userId.ToString(), _crypterSettings.SaltKey);
            return await Task.FromResult(encryptId);
        }

        public async Task<long> DecryptUserId(string userIdentityId)
        {
            var id = long.Parse(_textCrypter.Decrypt(userIdentityId, _crypterSettings.SaltKey));
            return await Task.FromResult(id);
        }

        protected async Task<ApplicationAuthorizationPolicy> FindPolicyAsync(string policyName, CancellationToken cancellationToken)
        {
            var policyRequest = await _authorizationPolicyBusiness.FindByNameAsync(policyName); ;
            var authorizationPolicy = _mapper.Map<ApplicationAuthorizationPolicy>(policyRequest);
            return authorizationPolicy;
        }

        public async Task<bool> HasPolicyAsync(TUser user, string policyName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(policyName))
            {
                throw new ArgumentException("Value cannot be null or empty", nameof(policyName));
            }
            var policy = await FindPolicyAsync(policyName, cancellationToken);
            if (policy != null)
            {
                var isUserHasPolicy = await IsUserHasPolicyAsync(user.Id, policy.Id);
                return isUserHasPolicy;
            }
            return false;
        }

        protected async Task<bool> IsUserHasPolicyAsync(long userId, long policyId)
        {
            var isUserHasPolicy = await _userAuthorizationPolicyBusiness.IsUserHasAuthoricationPolicyAsync(userId, policyId);
            return isUserHasPolicy;
        }
    }
}