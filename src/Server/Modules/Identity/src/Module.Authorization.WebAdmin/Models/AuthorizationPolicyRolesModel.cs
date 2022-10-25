using Camino.Infrastructure.AspNetCore.Models;
using System.Collections.Generic;

namespace Module.Authorization.WebAdmin.Models
{
    public class AuthorizationPolicyRolesModel : PageListModel<RoleModel>
    {
        public AuthorizationPolicyRolesModel(IEnumerable<RoleModel> collections) : base(collections)
        {
            Filter = new RoleAuthorizationPolicyFilterModel();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
