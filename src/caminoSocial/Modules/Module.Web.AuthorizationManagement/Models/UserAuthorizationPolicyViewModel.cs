using System.Collections.Generic;

namespace Module.Web.AuthorizationManagement.Models
{
    public class UserAuthorizationPolicyViewModel
    {
        public long UserId { get; set; }
        public string AuthorizationPolicyName { get; set; }
        public long AuthorizationPolicyId { get; set; }
        public IEnumerable<long> CurrentUserIds { get; set; }
    }
}
