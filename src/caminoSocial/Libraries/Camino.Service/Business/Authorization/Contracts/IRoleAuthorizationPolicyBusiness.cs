using Camino.Service.Data.Filters;
using Camino.Service.Data.PageList;

namespace Camino.Service.Business.Authorization.Contracts
{
    public interface IRoleAuthorizationPolicyBusiness
    {
        AuthorizationPolicyRolesPageList GetAuthoricationPolicyRoles(long id, RoleAuthorizationPolicyFilter filter);
        bool Add(long roleId, long authorizationPolicyId, long loggedUserId);
        bool Delete(long roleId, long authorizationPolicyId);
    }
}
