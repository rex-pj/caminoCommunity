using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Authorization;
using Camino.Shared.Results.PageList;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Services.Authorization
{
    public interface IUserAuthorizationPolicyService
    {
        bool Create(long userId, long authorizationPolicyId, long loggedUserId);
        bool Delete(long userId, short authorizationPolicyId);
        AuthorizationPolicyUsersPageList GetAuthoricationPolicyUsers(long id, UserAuthorizationPolicyFilter filter);
        Task<UserAuthorizationPolicyResult> GetUserAuthoricationPolicyAsync(long userId, long policyId);
        Task<bool> IsUserHasAuthoricationPolicyAsync(long userId, long policyId);
    }
}
