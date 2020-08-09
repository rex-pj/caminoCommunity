using System.Collections.Generic;

namespace Module.Web.AuthorizationManagement.Models
{
    public class RoleAuthorizationPolicyViewModel
    {
        public long RoleId { get; set; }
        public string AuthorizationPolicyName { get; set; }
        public long AuthorizationPolicyId { get; set; }
        public IEnumerable<long> CurrentRoleIds { get; set; }
    }
}
