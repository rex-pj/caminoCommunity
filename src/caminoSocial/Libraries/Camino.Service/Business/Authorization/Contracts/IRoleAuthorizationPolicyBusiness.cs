using Camino.Service.Projections.Filters;
using Camino.Service.Projections.PageList;

namespace Camino.Service.Business.Authorization.Contracts
{
    public interface IRoleAuthorizationPolicyBusiness
    {
        AuthorizationPolicyRolesPageList GetAuthoricationPolicyRoles(long id, RoleAuthorizationPolicyFilter filter);
        bool Add(long roleId, long authorizationPolicyId, long loggedUserId);
        bool Delete(long roleId, long authorizationPolicyId);
    }
}
