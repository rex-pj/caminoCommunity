using System.Collections.Generic;

namespace Coco.Entities.Dtos.Auth
{
    public class UserRoleAuthorizationPoliciesDto
    {
        public long UserId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public IEnumerable<RoleAuthorizationPoliciesDto> Roles { get; set; }
        public IEnumerable<AuthorizationPolicyDto> AuthorizationPolicies { get; set; }
    }
}
