using AutoMapper;
using Camino.Application.Contracts.AppServices.Authentication;
using Camino.Application.Contracts.AppServices.Authentication.Dtos;
using Camino.Application.Contracts.AppServices.Authorization;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Application.Contracts.AppServices.Users;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Infrastructure.Identity.Options;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security.Claims;

namespace Camino.Infrastructure.Identity.Stores
{
    public class ApplicationUserStore<TUser> : UserStoreBase<TUser, ApplicationRole, long, ApplicationUserClaim,
        ApplicationUserRole, ApplicationUserLogin, ApplicationUserToken, ApplicationRoleClaim>,
        IUserPasswordStore<TUser>, IUserAuthenticationTokenStore<TUser>,
        IUserEncryptionStore<TUser>, IUserSecurityStampStore<TUser>,
        IUserPolicyStore<TUser>, IUserTokenStore<TUser>
        where TUser : ApplicationUser
    {
        private readonly IMapper _mapper;

        private readonly IUserAppService _userAppService;
        private readonly IAuthenticationAppService _authenticationAppService;
        private readonly IUserRoleAppService _userRoleAppService;
        private readonly IRoleAppService _roleService;
        private readonly IAuthorizationPolicyAppService _authorizationPolicyAppService;
        private readonly IUserAuthorizationPolicyAppService _userAuthorizationPolicyAppService;
        private readonly ITextEncryption _textCrypter;
        private readonly CrypterSettings _crypterSettings;
        private readonly JwtConfigOptions _jwtConfigOptions;

        public override IQueryable<TUser> Users { get; }

        public ApplicationUserStore(IdentityErrorDescriber describer, IUserAppService userAppService,
            IAuthenticationAppService authenticationAppService,
            IUserRoleAppService userRoleAppService, IRoleAppService roleAppService,
            IAuthorizationPolicyAppService authorizationPolicyAppService,
            IUserAuthorizationPolicyAppService userAuthorizationPolicyAppService, ITextEncryption textCrypter,
            IMapper mapper, IOptions<CrypterSettings> crypterSettings, IOptions<JwtConfigOptions> jwtConfigOptions)
            : base(describer)
        {
            _crypterSettings = crypterSettings.Value;
            _userAppService = userAppService;
            _authenticationAppService = authenticationAppService;
            _userRoleAppService = userRoleAppService;
            _roleService = roleAppService;
            _authorizationPolicyAppService = authorizationPolicyAppService;
            _userAuthorizationPolicyAppService = userAuthorizationPolicyAppService;
            _mapper = mapper;
            _textCrypter = textCrypter;
            _jwtConfigOptions = jwtConfigOptions.Value;
        }

        public override async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }

                await _userAppService.CreateAsync(new UserModifyRequest
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
            var user = await _userAppService.FindByIdAsync(userId);
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
                await _userAppService.SoftDeleteAsync(user.Id, user.UpdatedById);
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
            var user = await _userAppService.FindByUsernameAsync(normalizedUserName);
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
            await _userRoleAppService.CreateAsync(userRoleRequest);
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
            var userRole = await _userRoleAppService.FindUserRoleAsync(userId, roleId);
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

            var roles = (await _userRoleAppService.GetUserRolesAsync(user.Id))
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
                var users = await _userRoleAppService.GetUsersInRoleAsync(role.Id);
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
                    await _userRoleAppService.RemoveAsync(userRoleRequest);
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
                _authenticationAppService.CreateClaim(ruserClaimRequest);
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
            var user = await _userAppService.FindByEmailAsync(normalizedEmail);
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
            _authenticationAppService.CreateUserLogin(new UserLoginRequest
            {
                LoginProvider = userLogin.LoginProvider,
                ProviderDisplayName = userLogin.ProviderDisplayName,
                ProviderKey = userLogin.ProviderKey,
                UserId = userLogin.UserId,
            });
            return Task.FromResult(false);
        }

        protected override async Task<ApplicationUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var userLogin = await _authenticationAppService.FindUserLoginAsync(loginProvider, providerKey);
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
            var userLogin = await _authenticationAppService.FindUserLoginAsync(userId, loginProvider, providerKey);
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
            var userInfos = (await _authenticationAppService.GetUserLoginByUserIdAsync(user.Id))
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
                await _authenticationAppService.RemoveUserLoginAsync(new UserLoginRequest
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

            var userClaims = await _authenticationAppService.GetClaimsByUserIdAsync(user.Id);
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
            var claimUsers = await _authenticationAppService.GetUsersByClaimAsync(claimRequest);
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
                var userClaims = await _authenticationAppService.GetUserClaimsByClaimAsync(user.Id, claim.Value, claim.Type);
                foreach (var c in userClaims)
                {
                    await _authenticationAppService.RemoveClaimAsync(new UserClaimRequest
                    {
                        ClaimType = c.ClaimType,
                        ClaimValue = c.ClaimValue,
                        Id = c.Id,
                        UserId = c.UserId
                    });
                }
            }
        }

        public override async Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var token = await FindTokenByValueAsync(user, value, name);
            if (token == null)
            {
                await AddUserTokenAsync(new ApplicationUserToken
                {
                    ExpiryTime = DateTime.UtcNow.AddHours(_jwtConfigOptions.RefreshTokenHourExpires),
                    LoginProvider = loginProvider,
                    Name = name,
                    Value = value,
                    UserId = user.Id
                });
            }
            else
            {
                token.Value = value;
            }
        }

        protected override async Task AddUserTokenAsync(ApplicationUserToken token)
        {
            var userToken = new UserTokenRequest
            {
                LoginProvider = token.LoginProvider,
                Name = token.Name,
                Value = token.Value,
                UserId = token.UserId,
                ExpiryTime = token.ExpiryTime
            };
            await _authenticationAppService.CreateUserTokenAsync(userToken);
        }

        protected override async Task<ApplicationUserToken> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            var userToken = await _authenticationAppService.FindUserTokenAsync(user.Id, loginProvider, name);
            if (userToken == null)
            {
                return null;
            }

            return new ApplicationUserToken
            {
                Id = userToken.Id,
                LoginProvider = userToken.LoginProvider,
                Name = userToken.Name,
                UserId = userToken.UserId,
                Value = userToken.Value,
                ExpiryTime = userToken.ExpiryTime
            };
        }

        public async Task<ApplicationUserToken> FindTokenByValueAsync(TUser user, string value, string name)
        {
            var userToken = await _authenticationAppService.FindUserTokenByValueAsync(user.Id, value, name);
            if (userToken == null)
            {
                return null;
            }

            return new ApplicationUserToken
            {
                Id = userToken.Id,
                LoginProvider = userToken.LoginProvider,
                Name = userToken.Name,
                UserId = userToken.UserId,
                Value = userToken.Value,
                ExpiryTime = userToken.ExpiryTime
            };
        }

        protected override async Task RemoveUserTokenAsync(ApplicationUserToken token)
        {
            var userToken = new UserTokenRequest
            {
                LoginProvider = token.LoginProvider,
                Name = token.Name,
                Value = token.Value,
                UserId = token.UserId
            };
            await _authenticationAppService.RemoveUserTokenAsync(userToken);
        }

        public async Task RemoveAuthenticationTokenByValueAsync(long userId, string value)
        {
            await _authenticationAppService.RemoveAuthenticationTokenByValueAsync(new UserTokenRequest
            {
                Value = value,
                UserId = userId
            });
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
            await _authenticationAppService.ReplaceUserClaimAsync(user.Id, claimRequest, newClaimRequest);
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
                await _userAppService.UpdateAsync(new UserModifyRequest
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

        public async Task<string> EncryptUserIdAsync(long userId)
        {
            return await Task.FromResult(EncryptUserId(userId));
        }

        public async Task<long> DecryptUserIdAsync(string userIdentityId)
        {
            return await Task.FromResult(DecryptUserId(userIdentityId));
        }

        public string EncryptUserId(long userId)
        {
            var encryptId = _textCrypter.Encrypt(userId.ToString(), _crypterSettings.SaltKey);
            return encryptId;
        }

        public long DecryptUserId(string userIdentityId)
        {
            long.TryParse(_textCrypter.Decrypt(userIdentityId, _crypterSettings.SaltKey), out long id);
            return id;
        }

        protected async Task<ApplicationAuthorizationPolicy> FindPolicyAsync(string policyName, CancellationToken cancellationToken)
        {
            var policyRequest = await _authorizationPolicyAppService.FindByNameAsync(policyName);
            if (policyRequest == null)
            {
                return null;
            }

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
                return await IsUserHasPolicyAsync(user.Id, policy.Id);
            }
            return false;
        }

        protected async Task<bool> IsUserHasPolicyAsync(long userId, long policyId)
        {
            var isUserHasPolicy = await _userAuthorizationPolicyAppService.IsUserHasAuthoricationPolicyAsync(userId, policyId);
            return isUserHasPolicy;
        }
    }
}