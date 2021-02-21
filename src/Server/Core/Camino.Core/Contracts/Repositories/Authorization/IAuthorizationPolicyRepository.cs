using Camino.Shared.Requests.Authorization;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Authorization;
using Camino.Shared.Results.PageList;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Authorization
{
    public interface IAuthorizationPolicyRepository
    {
        long Create(AuthorizationPolicyRequest request);
        BasePageList<AuthorizationPolicyResult> Get(AuthorizationPolicyFilter filter);
        AuthorizationPolicyResult Find(short id);
        Task<bool> UpdateAsync(AuthorizationPolicyRequest request);
        Task<AuthorizationPolicyResult> FindByNameAsync(string name);
    }
}
