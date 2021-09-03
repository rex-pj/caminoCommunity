using Camino.Shared.Results.Identifiers;
using Camino.Shared.Requests.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.Results.Authorization;
using Camino.Shared.Requests.Authorization;
using Camino.Shared.Results.Authentication;

namespace Camino.Core.Contracts.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<UserResult> UpdatePasswordAsync(UserPasswordUpdateRequest request);
        IEnumerable<UserRoleResult> GetUserRoles(long userd);
        void CreateClaim(UserClaimRequest userClaim);
        Task<IList<UserClaimResult>> GetClaimsByUserIdAsync(long userId);
        Task<IList<UserClaimResult>> GetUserClaimsByClaimAsync(long userId, string claimValue, string claimType);
        void RemoveClaim(UserClaimRequest userClaim);
        Task ReplaceClaimAsync(long userId, ClaimRequest claim, ClaimRequest newClaim);
        Task<IList<UserResult>> GetUsersByClaimAsync(ClaimRequest claim);
        Task<UserLoginResult> FindUserLoginAsync(long userId, string loginProvider, string providerKey);
        Task<UserLoginResult> FindUserLoginAsync(string loginProvider, string providerKey);
        Task<IList<UserLoginResult>> GetUserLoginByUserIdAsync(long userId);
        void CreateUserLogin(UserLoginRequest request);
        Task RemoveUserLoginAsync(UserLoginRequest request);
        Task<UserTokenResult> FindUserTokenAsync(long userId, string loginProvider, string name);
        Task<UserTokenResult> FindUserTokenByValueAsync(long userId, string value, string name);
        void CreateUserToken(UserTokenRequest request);
        Task RemoveUserTokenAsync(UserTokenRequest request);

        Task RemoveAuthenticationTokenByValueAsync(UserTokenRequest request);
    }
}
