using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;

namespace Camino.Core.Contracts.Repositories.Authorization
{
    public interface IRoleAuthorizationPolicyRepository
    {
        AuthorizationPolicyRolesPageList GetAuthoricationPolicyRoles(long id, RoleAuthorizationPolicyFilter filter);
        bool Create(long roleId, long authorizationPolicyId, long loggedUserId);
        bool Delete(long roleId, long authorizationPolicyId);
    }
}
