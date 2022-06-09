namespace Camino.Core.Domains.Authorization.Repositories
{
    public interface IUserAuthorizationPolicyRepository
    {
        Task<bool> CreateAsync(UserAuthorizationPolicy userAuthorizationPolicy);
        Task<bool> DeleteAsync(long userId, short authorizationPolicyId);
        Task<bool> IsUserHasAuthoricationPolicyAsync(long userId, long policyId);
    }
}
