using Camino.Business.Dtos.Identity;

namespace Camino.Business.Contracts
{
    public interface IRoleAuthorizationPolicyBusiness
    {
        AuthorizationPolicyRolesDto GetAuthoricationPolicyRoles(short id);
        bool Add(byte roleId, short authorizationPolicyId, long loggedUserId);
        bool Delete(byte roleId, short authorizationPolicyId);
    }
}
