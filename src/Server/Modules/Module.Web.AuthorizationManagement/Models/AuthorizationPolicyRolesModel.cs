using Camino.Framework.Models;
using System.Collections.Generic;

namespace Module.Web.AuthorizationManagement.Models
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
