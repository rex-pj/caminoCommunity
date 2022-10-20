using Camino.Core.Domains.Authentication.Repositories;
using Camino.Application.Contracts.AppServices.Authentication.Dtos;
using Camino.Application.Contracts.AppServices.Authentication;
using Camino.Core.DependencyInjection;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Core.Validators;
using Camino.Application.Validators;
using Camino.Core.Domains;
using Camino.Core.Domains.Users;
using Camino.Core.Domains.Authorization;
using Camino.Core.Domains.Authentication;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Camino.Application.AppServices.Authentication
{
    public partial class AuthenticationAppService : IAuthenticationAppService, IScopedDependency
    {
        #region Fields/Properties
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IEntityRepository<User> _userEntityRepository;
        private readonly IEntityRepository<UserClaim> _userClaimEntityRepository;
        private readonly IEntityRepository<UserRole> _userRoleEntityRepository;
        private readonly BaseValidatorContext _validatorContext;
        private readonly IDbContext _dbContext;
        #endregion

        #region Ctor
        public AuthenticationAppService(IEntityRepository<User> userEntityRepository,
            IEntityRepository<UserClaim> userClaimEntityRepository,
            IEntityRepository<UserRole> userRoleEntityRepository,
            IUserClaimRepository userClaimRepository,
            IUserLoginRepository userLoginRepository,
             BaseValidatorContext validatorContext,
            IUserTokenRepository userTokenRepository,
            IDbContext dbContext)
        {
            _userEntityRepository = userEntityRepository;
            _userClaimEntityRepository = userClaimEntityRepository;
            _userRoleEntityRepository = userRoleEntityRepository;
            _userTokenRepository = userTokenRepository;
            _userLoginRepository = userLoginRepository;
            _userClaimRepository = userClaimRepository;
            _validatorContext = validatorContext;
            _dbContext = dbContext;
        }
        #endregion

        #region CRUD

        public async Task<UserResult> UpdatePasswordAsync(UserPasswordUpdateRequest request)
        {
            _validatorContext.SetValidator(new UserPasswordUpdateValidator());
            bool canUpdate = _validatorContext.Validate<UserPasswordUpdateRequest, bool>(request);
            if (!canUpdate)
            {
                throw new UnauthorizedAccessException();
            }

            var errors = _validatorContext.Errors;
            if (errors != null || errors.Any())
            {
                throw new ArgumentException(errors.FirstOrDefault().Message);
            }

            var user = await _userEntityRepository.FindAsync(x => x.Id == request.UserId);
            if (user == null)
            {
                return new UserResult();
            }
            user.PasswordHash = request.NewPassword;
            await _userEntityRepository.UpdateAsync(user);
            await _dbContext.SaveChangesAsync();
            return new UserResult()
            {
                Id = user.Id
            };
        }
        #endregion

        #region GET
        public IEnumerable<UserRoleResult> GetUserRoles(long userd)
        {
            var userRoles = (from user in _userEntityRepository.Table
                             join userRole in _userRoleEntityRepository.Table
                             on user.Id equals userRole.UserId into roles
                             from userRole in roles.DefaultIfEmpty()
                             where user.Id == userd
                             select new UserRoleResult()
                             {
                                 UserId = user.Id,
                                 RoleId = userRole.RoleId,
                                 RoleName = userRole.Role.Name
                             }).ToList();

            return userRoles;
        }
        #endregion

        #region Claim
        public async Task<int> CreateClaim(UserClaimRequest userClaim)
        {
            var claim = new UserClaim
            {
                ClaimType = userClaim.ClaimType,
                ClaimValue = userClaim.ClaimValue,
                UserId = userClaim.UserId
            };
            return await _userClaimRepository.CreateAsync(claim);
        }

        public async Task<IList<UserClaimResult>> GetClaimsByUserIdAsync(long userId)
        {
            var userClaims = (await _userClaimRepository.GetByUserIdAsync(userId))
                .Select(x => MapEntityToDto(x))
                .ToList();
            return userClaims;
        }

        public async Task<IList<UserClaimResult>> GetUserClaimsByClaimAsync(long userId, string claimValue, string claimType)
        {
            var userClaims = (await _userClaimRepository.GetByClaimAsync(userId, claimValue, claimType))
                .Select(x => MapEntityToDto(x))
                .ToList();
            return userClaims;
        }

        private UserClaimResult MapEntityToDto(UserClaim userClaim)
        {
            return new UserClaimResult
            {
                ClaimType = userClaim.ClaimType,
                Id = userClaim.Id,
                ClaimValue = userClaim.ClaimValue,
                UserId = userClaim.UserId
            };
        }

        public async Task<bool> RemoveClaimAsync(UserClaimRequest request)
        {
            var userClaim = new UserClaim
            {
                ClaimType = request.ClaimType,
                ClaimValue = request.ClaimValue,
                UserId = request.UserId,
                Id = request.Id
            };
            return await _userClaimRepository.RemoveAsync(userClaim);
        }

        public async Task ReplaceUserClaimAsync(long userId, ClaimRequest claim, ClaimRequest newClaim)
        {
            var matchedClaims = await _userClaimRepository.GetByClaimAsync(userId, claim.Value, claim.Type);
            foreach (var matchedClaim in matchedClaims)
            {
                matchedClaim.ClaimValue = newClaim.Value;
                matchedClaim.ClaimType = newClaim.Type;
            }
        }

        public async Task<IList<UserResult>> GetUsersByClaimAsync(ClaimRequest claim)
        {
            var existUserClaims = await _userClaimEntityRepository
                    .GetAsync(x => x.ClaimValue == claim.Value && x.ClaimType == claim.Type, x => new UserResult
                    {
                        Id = x.UserId,
                        DisplayName = x.User.DisplayName,
                        Lastname = x.User.Lastname,
                        Firstname = x.User.Firstname,
                        UserName = x.User.UserName,
                        Email = x.User.Email,
                        IsEmailConfirmed = x.User.IsEmailConfirmed,
                        StatusId = x.User.StatusId
                    });

            return existUserClaims;
        }
        #endregion

        #region UserLogin
        public async Task<UserLoginResult> FindUserLoginAsync(long userId, string loginProvider, string providerKey)
        {
            var existing = await _userLoginRepository.FindAsync(userId, loginProvider, providerKey);
            if (existing == null)
            {
                return new UserLoginResult();
            }

            return MapEntityToDto(existing);
        }

        private UserLoginResult MapEntityToDto(UserLogin userLogin)
        {
            return new UserLoginResult
            {
                Id = userLogin.Id,
                LoginProvider = userLogin.LoginProvider,
                ProviderDisplayName = userLogin.ProviderDisplayName,
                ProviderKey = userLogin.ProviderKey,
                UserId = userLogin.UserId
            };
        }

        public async Task<UserLoginResult> FindUserLoginAsync(string loginProvider, string providerKey)
        {
            var existing = await _userLoginRepository.FindAsync(loginProvider, providerKey);
            if (existing == null)
            {
                return new UserLoginResult();
            }

            return MapEntityToDto(existing);
        }

        public async Task<IList<UserLoginResult>> GetUserLoginByUserIdAsync(long userId)
        {
            var userLogins = await _userLoginRepository.GetByUserIdAsync(userId);
            if (userLogins == null)
            {
                return new List<UserLoginResult>();
            }

            return userLogins.Select(x => MapEntityToDto(x)).ToList();
        }

        public void CreateUserLogin(UserLoginRequest request)
        {
            var userLogin = new UserLogin()
            {
                LoginProvider = request.LoginProvider,
                ProviderDisplayName = request.ProviderDisplayName,
                ProviderKey = request.ProviderKey,
                UserId = request.UserId
            };
            _userLoginRepository.CreateAsync(userLogin);
        }

        public async Task RemoveUserLoginAsync(UserLoginRequest request)
        {
            await _userLoginRepository.RemoveAsync(request.LoginProvider, request.ProviderKey, request.ProviderDisplayName, request.UserId);
        }
        #endregion

        #region UserToken
        public async Task<UserTokenResult> FindUserTokenAsync(long userId, string loginProvider, string name)
        {
            var userToken = await _userTokenRepository.FindAsync(userId, loginProvider, name);
            return MapEntityToDto(userToken);
        }

        public async Task<UserTokenResult> FindUserTokenByValueAsync(long userId, string value, string name)
        {
            var userToken = await _userTokenRepository.FindByValueAsync(userId, value, name);
            return MapEntityToDto(userToken);
        }

        private UserTokenResult MapEntityToDto(UserToken userToken)
        {
            if (userToken == null)
            {
                return null;
            }

            return new UserTokenResult
            {
                Id = userToken.Id,
                UserId = userToken.UserId,
                LoginProvider = userToken.LoginProvider,
                Name = userToken.Name,
                Value = userToken.Value,
                ExpiryTime = userToken.ExpiryTime
            };
        }

        public async Task<long> CreateUserTokenAsync(UserTokenRequest request)
        {
            var userToken = new UserToken
            {
                LoginProvider = request.LoginProvider,
                Name = request.Name,
                Value = request.Value,
                UserId = request.UserId,
                ExpiryTime = request.ExpiryTime
            };
            return await _userTokenRepository.CreateAsync(userToken);
        }

        public async Task RemoveUserTokenAsync(UserTokenRequest request)
        {
            await _userTokenRepository.RemoveAsync(request.LoginProvider, request.Value, request.Name, request.UserId);
        }

        public async Task RemoveAuthenticationTokenByValueAsync(UserTokenRequest request)
        {
            await _userTokenRepository.RemoveByValueAsync(request.Value, request.UserId);
        }
        #endregion
    }
}
