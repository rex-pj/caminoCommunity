using Camino.Business.Dtos.General;
using Camino.Business.Dtos.Identity;

namespace Camino.Business.Contracts
{
    public interface IRoleAuthorizationPolicyBusiness
    {
        AuthorizationPolicyRolesDto GetAuthoricationPolicyRoles(long id, RoleAuthorizationPolicyFilterDto filter);
        bool Add(long roleId, long authorizationPolicyId, long loggedUserId);
        bool Delete(long roleId, long authorizationPolicyId);
    }
}
