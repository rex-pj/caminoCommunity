using System.Collections.Generic;

namespace Camino.Service.Data.Identity
{
    public class UserRoleAuthorizationPoliciesResult
    {
        public long UserId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public IEnumerable<RoleAuthorizationPoliciesResult> Roles { get; set; }
        public IEnumerable<AuthorizationPolicyResult> AuthorizationPolicies { get; set; }
    }
}
