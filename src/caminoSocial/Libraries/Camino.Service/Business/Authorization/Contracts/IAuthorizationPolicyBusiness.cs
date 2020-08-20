using Camino.Service.Data.Filters;
using Camino.Service.Data.Identity;
using Camino.Service.Data.Page;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authorization.Contracts
{
    public interface IAuthorizationPolicyBusiness
    {
        long Add(AuthorizationPolicyResult authorizationPolicy);
        PageList<AuthorizationPolicyResult> Get(AuthorizationPolicyFilter filter);
        AuthorizationPolicyResult Find(short id);
        AuthorizationPolicyResult Update(AuthorizationPolicyResult policy);
        Task<AuthorizationPolicyResult> FindByNameAsync(string name);
    }
}
