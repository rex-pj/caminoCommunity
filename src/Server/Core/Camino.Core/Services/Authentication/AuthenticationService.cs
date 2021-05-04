using Camino.Shared.Results.Identifiers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Shared.Requests.Authentication;
using Camino.Shared.Results.Authorization;
using Camino.Core.Contracts.Services.Authentication;
using Camino.Core.Contracts.Repositories.Authentication;
using Camino.Shared.Requests.Authorization;
using Camino.Shared.Results.Authentication;

namespace Camino.Services.Authentication
{
    public partial class AuthenticationService : IAuthenticationService
    {
        #region Fields/Properties
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        #endregion

        #region Ctor
        public AuthenticationService(IAuthenticationRepository authenticationRepository, IUserClaimRepository userClaimRepository,
            IUserLoginRepository userLoginRepository, IUserTokenRepository userTokenRepository)
        {
            _userTokenRepository = userTokenRepository;
            _userLoginRepository = userLoginRepository;
            _authenticationRepository = authenticationRepository;
            _userClaimRepository = userClaimRepository;
        }
        #endregion

        #region CRUD

        public async Task<UserResult> UpdatePasswordAsync(UserPasswordUpdateRequest request)
        {
            return await _authenticationRepository.UpdatePasswordAsync(request);
        }
        #endregion

        #region GET
        public IEnumerable<UserRoleResult> GetUserRoles(long userd)
        {
            return _authenticationRepository.GetUserRoles(userd);
        }
        #endregion

        #region Claim
        public void CreateClaim(UserClaimRequest userClaim)
        {
            _userClaimRepository.Create(userClaim);
        }

        public async Task<IList<UserClaimResult>> GetClaimsByUserIdAsync(long userId)
        {
            var userClaims = await _userClaimRepository.GetByUserIdAsync(userId);
            return userClaims;
        }

        public async Task<IList<UserClaimResult>> GetUserClaimsByClaimAsync(long userId, string claimValue, string claimType)
        {
            var userClaims = await _userClaimRepository.GetByClaimAsync(userId, claimValue, claimType);
            return userClaims;
        }

        public void RemoveClaim(UserClaimRequest userClaim)
        {
            _userClaimRepository.Remove(userClaim);
        }

        public async Task ReplaceClaimAsync(long userId, ClaimRequest claim, ClaimRequest newClaim)
        {
            await _userClaimRepository.ReplaceClaimAsync(userId, claim, newClaim);
        }

        public async Task<IList<UserResult>> GetUsersByClaimAsync(ClaimRequest claim)
        {
            var existUserClaims = await _userClaimRepository.GetUsersByClaimAsync(claim);
            return existUserClaims;
        }
        #endregion

        #region UserLogin
        public async Task<UserLoginResult> FindUserLoginAsync(long userId, string loginProvider, string providerKey)
        {
            return await _userLoginRepository.FindAsync(userId, loginProvider, providerKey);
        }

        public async Task<UserLoginResult> FindUserLoginAsync(string loginProvider, string providerKey)
        {
            return await _userLoginRepository.FindAsync(loginProvider, providerKey);
        }

        public async Task<IList<UserLoginResult>> GetUserLoginByUserIdAsync(long userId)
        {
            return await _userLoginRepository.GetByUserIdAsync(userId);
        }

        public void CreateUserLogin(UserLoginRequest request)
        {
            _userLoginRepository.Create(request);
        }

        public async Task RemoveUserLoginAsync(UserLoginRequest request)
        {
            await _userLoginRepository.RemoveAsync(request);
        }
        #endregion

        #region UserToken
        public async Task<UserTokenResult> FindUserTokenAsync(long userId, string loginProvider, string name)
        {
            var userToken = await _userTokenRepository.FindAsync(userId, loginProvider, name);
            return userToken;
        }

        public void CreateUserToken(UserTokenRequest request)
        {
            _userTokenRepository.Create(request);
        }

        public void RemoveUserToken(UserTokenRequest request)
        {
            _userTokenRepository.Remove(request);
        }
        #endregion
    }
}
