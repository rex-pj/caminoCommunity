using System.Threading.Tasks;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using Camino.Core.Contracts.Services.Authorization;
using Camino.Shared.Results.Authorization;
using Camino.Core.Contracts.Repositories.Authorization;
using Camino.Shared.Requests.Authorization;

namespace Camino.Services.Authorization
{
    public class AuthorizationPolicyService : IAuthorizationPolicyService
    {
        private readonly IAuthorizationPolicyRepository _authorizationPolicyRepository;

        public AuthorizationPolicyService(IAuthorizationPolicyRepository authorizationPolicyRepository)
        {
            _authorizationPolicyRepository = authorizationPolicyRepository;
        }

        public AuthorizationPolicyResult Find(short id)
        {
            var authorizationPolicy = _authorizationPolicyRepository.Find(id);
            return authorizationPolicy;
        }

        public async Task<AuthorizationPolicyResult> FindByNameAsync(string name)
        {
            var policy = await _authorizationPolicyRepository.FindByNameAsync(name);
            return policy;
        }

        public BasePageList<AuthorizationPolicyResult> Get(AuthorizationPolicyFilter filter)
        {
            return _authorizationPolicyRepository.Get(filter);
        }

        public long Create(AuthorizationPolicyRequest request)
        {
            return _authorizationPolicyRepository.Create(request);
        }

        public async Task<bool> UpdateAsync(AuthorizationPolicyRequest request)
        {
            return await _authorizationPolicyRepository.UpdateAsync(request);
        }

    }
}
