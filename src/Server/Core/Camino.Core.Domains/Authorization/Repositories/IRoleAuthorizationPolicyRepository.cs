namespace Camino.Core.Domains.Authorization.Repositories
{
    public interface IRoleAuthorizationPolicyRepository
    {
        Task<bool> CreateAsynx(RoleAuthorizationPolicy roleAuthorizationPolicy);
        Task<bool> DeleteAsync(long roleId, long authorizationPolicyId);
    }
}
