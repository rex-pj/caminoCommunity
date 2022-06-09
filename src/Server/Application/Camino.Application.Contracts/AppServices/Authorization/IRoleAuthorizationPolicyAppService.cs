using Camino.Application.Contracts.AppServices.Authorization.Dtos;

namespace Camino.Application.Contracts.AppServices.Authorization
{
    public interface IRoleAuthorizationPolicyAppService
    {
        Task<bool> CreateAsync(long roleId, long authorizationPolicyId, long loggedUserId);
        Task<bool> DeleteAsync(long roleId, long authorizationPolicyId);
        Task<AuthorizationPolicyRolesPageList> GetPageListAsync(long id, RoleAuthorizationPolicyFilter filter);
    }
}
