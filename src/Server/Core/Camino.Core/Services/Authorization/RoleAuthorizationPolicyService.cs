using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using Camino.Core.Contracts.Services.Authorization;
using Camino.Core.Contracts.Repositories.Authorization;
using Camino.Core.Contracts.DependencyInjection;

namespace Camino.Services.Authorization
{
    public class RoleAuthorizationPolicyService : IRoleAuthorizationPolicyService, IScopedDependency
    {
        private readonly IRoleAuthorizationPolicyRepository _roleAuthorizationPolicyRepository;

        public RoleAuthorizationPolicyService(IRoleAuthorizationPolicyRepository roleAuthorizationPolicyRepository)
        {
            _roleAuthorizationPolicyRepository = roleAuthorizationPolicyRepository;
        }

        public bool Create(long roleId, long authorizationPolicyId, long loggedUserId)
        {
            return _roleAuthorizationPolicyRepository.Create(roleId, authorizationPolicyId, loggedUserId);
        }

        public bool Delete(long roleId, long authorizationPolicyId)
        {
            return _roleAuthorizationPolicyRepository.Delete(roleId, authorizationPolicyId);
        }

        public AuthorizationPolicyRolesPageList GetAuthoricationPolicyRoles(long id, RoleAuthorizationPolicyFilter filter)
        {
            return _roleAuthorizationPolicyRepository.GetAuthoricationPolicyRoles(id, filter);
        }
    }
}
