using System.Collections.Generic;

namespace Coco.Business.Dtos.Identity
{
    public class AuthorizationPolicyRolesDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<RoleDto> AuthorizationPolicyRoles { get; set; }
    }
}
