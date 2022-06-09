using Camino.Application.Contracts.AppServices.Authentication.Dtos;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Application.Contracts.AppServices.Users.Dtos;

namespace Camino.Application.Contracts.AppServices.Authentication
{
    public interface IAuthenticationAppService
    {
        Task<int> CreateClaim(UserClaimRequest userClaim);
        void CreateUserLogin(UserLoginRequest request);
        Task<long> CreateUserTokenAsync(UserTokenRequest request);
        Task<UserLoginResult> FindUserLoginAsync(long userId, string loginProvider, string providerKey);
        Task<UserLoginResult> FindUserLoginAsync(string loginProvider, string providerKey);
        Task<UserTokenResult> FindUserTokenAsync(long userId, string loginProvider, string name);
        Task<UserTokenResult> FindUserTokenByValueAsync(long userId, string value, string name);
        Task<IList<UserClaimResult>> GetClaimsByUserIdAsync(long userId);
        Task<IList<UserClaimResult>> GetUserClaimsByClaimAsync(long userId, string claimValue, string claimType);
        Task<IList<UserLoginResult>> GetUserLoginByUserIdAsync(long userId);
        IEnumerable<UserRoleResult> GetUserRoles(long userd);
        Task<IList<UserResult>> GetUsersByClaimAsync(ClaimRequest claim);
        Task RemoveAuthenticationTokenByValueAsync(UserTokenRequest request);
        Task<bool> RemoveClaimAsync(UserClaimRequest userClaim);
        Task RemoveUserLoginAsync(UserLoginRequest request);
        Task RemoveUserTokenAsync(UserTokenRequest request);
        Task ReplaceUserClaimAsync(long userId, ClaimRequest claim, ClaimRequest newClaim);
        Task<UserResult> UpdatePasswordAsync(UserPasswordUpdateRequest request);
    }
}
