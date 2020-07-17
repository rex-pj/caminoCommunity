using Camino.Business.Dtos.Identity;
using System.Collections.Generic;

namespace Camino.Business.Contracts
{
    public interface IAuthorizationPolicyBusiness
    {
        long Add(AuthorizationPolicyDto authorizationPolicy);
        List<AuthorizationPolicyDto> GetFull();
        AuthorizationPolicyDto Find(short id);
        AuthorizationPolicyDto Update(AuthorizationPolicyDto policy);
        AuthorizationPolicyDto FindByName(string name);
    }
}
