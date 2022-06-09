using Camino.Application.Contracts.AppServices.Authorization.Dtos;

namespace Camino.Application.Contracts.AppServices.Authorization
{
    public interface IUserAuthorizationPolicyAppService
    {
        Task<bool> CreateAsync(long userId, long authorizationPolicyId, long loggedUserId);
        Task<bool> DeleteAsync(long userId, short authorizationPolicyId);
        AuthorizationPolicyUsersPageList GetAuthoricationPolicyUsers(long id, UserAuthorizationPolicyFilter filter);
        Task<UserAuthorizationPolicyResult> GetUserAuthoricationPolicyAsync(long userId, long policyId);
        Task<bool> IsUserHasAuthoricationPolicyAsync(long userId, long policyId);
    }
}
