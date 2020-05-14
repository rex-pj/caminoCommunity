using System.Collections.Generic;

namespace Coco.Entities.Dtos.Auth
{
    public class RoleAuthorizationPoliciesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<AuthorizationPolicyDto> AuthorizationPolicies { get; set; }
    }
}
