using Coco.Entities.Dtos.Auth;
using System.Collections.Generic;

namespace Coco.Business.Contracts
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
