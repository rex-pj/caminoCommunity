using Camino.Service.Data.Filters;
using Camino.Service.Data.Identity;
using Camino.Service.Data.PageList;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authorization.Contracts
{
    public interface IAuthorizationPolicyBusiness
    {
        long Add(AuthorizationPolicyProjection authorizationPolicy);
        BasePageList<AuthorizationPolicyProjection> Get(AuthorizationPolicyFilter filter);
        AuthorizationPolicyProjection Find(short id);
        AuthorizationPolicyProjection Update(AuthorizationPolicyProjection policy);
        Task<AuthorizationPolicyProjection> FindByNameAsync(string name);
    }
}
