using System.Collections.Generic;

namespace Camino.Service.Data.Identity
{
    public class RoleAuthorizationPoliciesResult
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<AuthorizationPolicyResult> AuthorizationPolicies { get; set; }
    }
}
