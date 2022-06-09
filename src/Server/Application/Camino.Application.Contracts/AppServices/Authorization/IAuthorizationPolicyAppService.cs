using Camino.Application.Contracts.AppServices.Authorization.Dtos;

namespace Camino.Application.Contracts.AppServices.Authorization
{
    public interface IAuthorizationPolicyAppService
    {
        Task<long> CreateAsync(AuthorizationPolicyRequest request);
        Task<AuthorizationPolicyResult> FindAsync(short id);
        Task<AuthorizationPolicyResult> FindByNameAsync(string name);
        Task<BasePageList<AuthorizationPolicyResult>> GetAsync(AuthorizationPolicyFilter filter);
        Task<bool> UpdateAsync(AuthorizationPolicyRequest request);
    }
}
