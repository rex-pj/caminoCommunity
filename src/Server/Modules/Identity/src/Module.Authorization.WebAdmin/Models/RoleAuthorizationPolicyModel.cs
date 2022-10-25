using System.Collections.Generic;

namespace Module.Authorization.WebAdmin.Models
{
    public class RoleAuthorizationPolicyModel
    {
        public long RoleId { get; set; }
        public string AuthorizationPolicyName { get; set; }
        public long AuthorizationPolicyId { get; set; }
        public IEnumerable<long> CurrentRoleIds { get; set; }
    }
}
