using System.Collections.Generic;

namespace Camino.Service.Data.Identity
{
    public class UserRoleAuthorizationPoliciesProjection
    {
        public long UserId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public IEnumerable<RoleAuthorizationPoliciesProjection> Roles { get; set; }
        public IEnumerable<AuthorizationPolicyProjection> AuthorizationPolicies { get; set; }
    }
}
