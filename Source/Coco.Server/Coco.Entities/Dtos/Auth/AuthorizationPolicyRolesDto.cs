using System.Collections.Generic;

namespace Coco.Entities.Dtos.Auth
{
    public class AuthorizationPolicyRolesDto
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<RoleDto> AuthorizationPolicyRoles { get; set; }
    }
}
