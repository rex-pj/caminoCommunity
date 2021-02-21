using System.Threading.Tasks;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using Camino.Core.Contracts.Services.Authorization;
using Camino.Shared.Results.Authorization;
using Camino.Core.Contracts.Repositories.Authorization;

namespace Camino.Services.Authorization
{
    public class UserAuthorizationPolicyService : IUserAuthorizationPolicyService
    {
        private readonly IUserAuthorizationPolicyRepository _userAuthorizationPolicyRepository;

        public UserAuthorizationPolicyService(IUserAuthorizationPolicyRepository userAuthorizationPolicyRepository)
        {
            _userAuthorizationPolicyRepository = userAuthorizationPolicyRepository;
        }

        public bool Create(long userId, long authorizationPolicyId, long loggedUserId)
        {
            return _userAuthorizationPolicyRepository.Create(userId, authorizationPolicyId, loggedUserId);
        }

        public bool Delete(long userId, short authorizationPolicyId)
        {
            return _userAuthorizationPolicyRepository.Delete(userId, authorizationPolicyId);
        }

        public AuthorizationPolicyUsersPageList GetAuthoricationPolicyUsers(long id, UserAuthorizationPolicyFilter filter)
        {
            return _userAuthorizationPolicyRepository.GetAuthoricationPolicyUsers(id, filter);
        }

        public async Task<UserAuthorizationPolicyResult> GetUserAuthoricationPolicyAsync(long userId, long policyId)
        {
            return await _userAuthorizationPolicyRepository.GetUserAuthoricationPolicyAsync(userId, policyId);
        }

        public async Task<bool> IsUserHasAuthoricationPolicyAsync(long userId, long policyId)
        {
            return await _userAuthorizationPolicyRepository.IsUserHasAuthoricationPolicyAsync(userId, policyId);
        }
    }
}
