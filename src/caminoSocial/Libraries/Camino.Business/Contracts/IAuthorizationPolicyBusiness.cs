using Camino.Business.Dtos.General;
using Camino.Business.Dtos.Identity;
using System.Threading.Tasks;

namespace Camino.Business.Contracts
{
    public interface IAuthorizationPolicyBusiness
    {
        long Add(AuthorizationPolicyDto authorizationPolicy);
        PageListDto<AuthorizationPolicyDto> Get(int page, int pageSize);
        AuthorizationPolicyDto Find(short id);
        AuthorizationPolicyDto Update(AuthorizationPolicyDto policy);
        Task<AuthorizationPolicyDto> FindByNameAsync(string name);
    }
}
