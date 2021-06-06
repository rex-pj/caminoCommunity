using AutoMapper;
using Camino.Core.Domain.Identities;
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
using Camino.Core.Contracts.Services.Users;
using Camino.Core.Contracts.Services.Authentication;
using Camino.Core.Contracts.Services.Authorization;
using Camino.Core.Contracts.IdentityManager;
using Camino.Shared.Requests.Authentication;
using Camino.Shared.Requests.Identifiers;
using Camino.Shared.Requests.Authorization;

namespace Camino.IdentityManager.Contracts.Stores
{
    public class ApplicationUserStore<TUser> : UserStoreBase<TUser, ApplicationRole, long, ApplicationUserClaim,
        ApplicationUserRole, ApplicationUserLogin, ApplicationUserToken, ApplicationRoleClaim>,
        IUserPasswordStore<TUser>, IUserAuthenticationTokenStore<TUser>,
        IUserEncryptionStore<TUser>, IUserSecurityStampStore<TUser>,
        IUserPolicyStore<TUser>
        where TUser : ApplicationUser
    {
        private readonly IMapper _mapper;

        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRoleService _userRoleService;
        private readonly IRoleService _roleService;
        private readonly IAuthorizationPolicyService _authorizationPolicyService;
        private readonly IUserAuthorizationPolicyService _userAuthorizationPolicyService;
        private readonly ITextEncryption _textCrypter;
        private readonly CrypterSettings _crypterSettings;

        public override IQueryable<TUser> Users { get; }

        public ApplicationUserStore(IdentityErrorDescriber describer, IUserService userService,
            IAuthenticationService authenticationService, IUserRoleService userRoleService, IRoleService roleService,
            IAuthorizationPolicyService authorizationPolicyService,
            IUserAuthorizationPolicyService userAuthorizationPolicyService, ITextEncryption textCrypter,
            IMapper mapper, IOptions<CrypterSettings> crypterSettings)
            : base(describer)
        {
            _crypterSettings = crypterSettings.Value;
            _userService = userService;
            _authenticationService = authenticationService;
            _userRoleService = userRoleService;
            _roleService = roleService;
            _authorizationPolicyService = authorizationPolicyService;
            _userAuthorizationPolicyService = userAuthorizationPolicyService;
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

                await _userService.CreateAsync(new UserModifyRequest
                {
                    Address = user.Address,
                    BirthDate = user.BirthDate,
                    CountryId = user.CountryId,
                    CreatedById = user.CreatedById,
                    Description = user.Description,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Firstname = user.Firstname,
                    GenderId = user.GenderId,
                    Id = user.Id,
                    IsEmailConfirmed = user.EmailConfirmed,
                    Lastname = user.Lastname,
                    PasswordHash = user.PasswordHash,
                    PhoneNumber = user.PhoneNumber,
                    SecurityStamp = user.SecurityStamp,
                    StatusId = user.StatusId,
                    UpdatedById = user.UpdatedById,
                    UserName = user.UserName
                });

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
            var user = await FindUserAsync(id, cancellationToken);
            return user;
        }

        protected override async Task<TUser> FindUserAsync(long userId, CancellationToken cancellationToken)
        {
            var user = await _userService.FindByIdAsync(userId);
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
                await _userService.DeleteAsync(user.Id);
            }
            // Todo: check DbUpdateConcurrencyException
            catch (Exception)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        public override async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            var user = await _userService.FindByUsernameAsync(normalizedUserName);
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
            var userRoleRequest = new UserRoleRequest
            {
                RoleId = applicationUserRole.RoleId,
                UserId = applicationUserRole.UserId
            };
            _userRoleService.Create(userRoleRequest);
        }

        protected override async Task<ApplicationRole> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var role = await _roleService.FindByNameAsync(normalizedRoleName);
            return new ApplicationRole
            {
                CreatedById = role.CreatedById,
                ConcurrencyStamp = role.ConcurrencyStamp,
                CreatedByName = role.CreatedByName,
                CreatedDate = role.CreatedDate,
                Description = role.Description,
                Id = role.Id,
                Name = role.Name,
                UpdatedById = role.UpdatedById,
                UpdatedByName = role.UpdatedByName,
                UpdatedDate = role.UpdatedDate
            };
        }

        protected override async Task<ApplicationUserRole> FindUserRoleAsync(long userId, long roleId, CancellationToken cancellationToken)
        {
            var userRole = await _userRoleService.FindUserRoleAsync(userId, roleId);
            return new ApplicationUserRole
            {
                RoleId = userRole.RoleId,
                RoleName = userRole.RoleName,
                UserId = userRole.UserId
            };
        }

        public override async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var roles = (await _userRoleService.GetUserRolesAsync(user.Id))
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
                var users = await _userRoleService.GetUsersInRoleAsync(role.Id);
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
                    var userRoleRequest = new UserRoleRequest
                    {
                        UserId = userRole.UserId,
                        RoleId = userRole.RoleId
                    };
                    _userRoleService.Remove(userRoleRequest);
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
                var userClaim = CreateUserClaim(user, claim);
                var ruserClaimRequest = new UserClaimRequest
                {
                    ClaimType = userClaim.ClaimType,
                    ClaimValue = userClaim.ClaimValue,
                    UserId = userClaim.UserId
                };
                _authenticationService.CreateClaim(ruserClaimRequest);
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
            var user = await _userService.FindByEmailAsync(normalizedEmail);
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
            var userLoginRequest = new UserLoginRequest
            {
                LoginProvider = userLogin.LoginProvider,
                ProviderDisplayName = userLogin.ProviderDisplayName,
                ProviderKey = userLogin.ProviderKey,
                UserId = userLogin.UserId,
            };
            _authenticationService.CreateUserLogin(userLoginRequest);
            return Task.FromResult(false);
        }

        protected override async Task<ApplicationUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var userLogin = await _authenticationService.FindUserLoginAsync(loginProvider, providerKey);
            return new ApplicationUserLogin
            {
                LoginProvider = userLogin.LoginProvider,
                ProviderDisplayName = userLogin.ProviderDisplayName,
                ProviderKey = userLogin.ProviderKey,
                UserId = userLogin.UserId
            };
        }

        protected override async Task<ApplicationUserLogin> FindUserLoginAsync(long userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var userLogin = await _authenticationService.FindUserLoginAsync(userId, loginProvider, providerKey);
            return new ApplicationUserLogin
            {
                LoginProvider = userLogin.LoginProvider,
                ProviderDisplayName = userLogin.ProviderDisplayName,
                ProviderKey = userLogin.ProviderKey,
                UserId = userLogin.UserId
            };
        }

        public override async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            var userInfos = (await _authenticationService.GetUserLoginByUserIdAsync(user.Id))
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
                await _authenticationService.RemoveUserLoginAsync(new UserLoginRequest
                {
                    LoginProvider = entry.LoginProvider,
                    ProviderDisplayName = entry.ProviderDisplayName,
                    ProviderKey = entry.ProviderKey,
                    UserId = entry.UserId
                });
            }
        }

        public override async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userClaims = await _authenticationService.GetClaimsByUserIdAsync(user.Id);
            var claims = userClaims.Select(x => new ApplicationUserClaim
            {
                ClaimType = x.ClaimType,
                ClaimValue = x.ClaimValue,
                Id = x.Id,
                UserId = x.UserId
            });
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

            var claimRequest = new ClaimRequest
            {
                Type = claim.Type,
                Value = claim.Value
            };
            var claimUsers = await _authenticationService.GetUsersByClaimAsync(claimRequest);
            var applicationUserClaims = _mapper.Map<IList<TUser>>(claimUsers);

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
                var userClaims = await _authenticationService.GetUserClaimsByClaimAsync(user.Id, claim.Value, claim.Type);
                foreach (var c in userClaims)
                {
                    _authenticationService.RemoveClaim(new UserClaimRequest
                    {
                        ClaimType = c.ClaimType,
                        ClaimValue = c.ClaimValue,
                        Id = c.Id,
                        UserId = c.UserId
                    });
                }
            }
        }

        protected override Task AddUserTokenAsync(ApplicationUserToken token)
        {
            var userToken = new UserTokenRequest
            {
                LoginProvider = token.LoginProvider,
                Name = token.Name,
                Value = token.Value,
                UserId = token.UserId
            };
            _authenticationService.CreateUserToken(userToken);
            return Task.CompletedTask;
        }

        protected override async Task<ApplicationUserToken> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            var userToken = await _authenticationService.FindUserTokenAsync(user.Id, loginProvider, name);
            return new ApplicationUserToken
            {
                LoginProvider = userToken.LoginProvider,
                Name = userToken.Name,
                UserId = userToken.UserId,
                Value = userToken.Value
            };
        }

        protected override Task RemoveUserTokenAsync(ApplicationUserToken token)
        {
            var userToken = new UserTokenRequest
            {
                LoginProvider = token.LoginProvider,
                Name = token.Name,
                Value = token.Value,
                UserId = token.UserId
            };
            _authenticationService.RemoveUserToken(userToken);
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

            var claimRequest = new ClaimRequest
            {
                Type = claim.Type,
                Value = claim.Value
            };

            var newClaimRequest = new ClaimRequest
            {
                Type = newClaim.Type,
                Value = newClaim.Value
            };
            await _authenticationService.ReplaceClaimAsync(user.Id, claimRequest, newClaimRequest);
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
                await _userService.UpdateAsync(new UserModifyRequest
                {
                    Address = user.Address,
                    BirthDate = user.BirthDate,
                    CountryId = user.CountryId,
                    CreatedById = user.CreatedById,
                    Description = user.Description,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Firstname = user.Firstname,
                    GenderId = user.GenderId,
                    Id = user.Id,
                    IsEmailConfirmed = user.EmailConfirmed,
                    Lastname = user.Lastname,
                    PasswordHash = user.PasswordHash,
                    PhoneNumber = user.PhoneNumber,
                    SecurityStamp = user.SecurityStamp,
                    StatusId = user.StatusId,
                    UpdatedById = user.UpdatedById,
                    UserName = user.UserName
                });
            }
            // todo: check DbUpdateConcurrencyException
            catch (Exception)
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
            var policyRequest = await _authorizationPolicyService.FindByNameAsync(policyName);
            var authorizationPolicy = new ApplicationAuthorizationPolicy
            {
                Description = policyRequest.Description,
                Name = policyRequest.Name,
                Id = policyRequest.Id
            };
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
            var isUserHasPolicy = await _userAuthorizationPolicyService.IsUserHasAuthoricationPolicyAsync(userId, policyId);
            return isUserHasPolicy;
        }
    }
}