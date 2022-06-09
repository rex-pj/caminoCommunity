namespace Camino.Core.Domains.Authorization.Repositories
{
    public interface IAuthorizationPolicyRepository
    {
        Task<long> CreateAsync(AuthorizationPolicy request);
        Task<AuthorizationPolicy> FindAsync(short id);
        Task<bool> UpdateAsync(AuthorizationPolicy request);
        Task<AuthorizationPolicy> FindByNameAsync(string name);
    }
}
