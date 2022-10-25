using System.Collections.Generic;

namespace Module.Authorization.WebAdmin.Models
{
    public class UserAuthorizationPolicyModel
    {
        public long UserId { get; set; }
        public string AuthorizationPolicyName { get; set; }
        public long AuthorizationPolicyId { get; set; }
        public IEnumerable<long> CurrentUserIds { get; set; }
    }
}
