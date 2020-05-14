using System.Collections.Generic;

namespace Coco.Management.Models
{
    public class AuthorizationPolicyRolesViewModel
    {
        public AuthorizationPolicyRolesViewModel()
        {
            AuthorizationPolicyRoles = new List<RoleViewModel>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte RoleId { get; set; }

        public IEnumerable<RoleViewModel> AuthorizationPolicyRoles { get; set; }
    }
}
