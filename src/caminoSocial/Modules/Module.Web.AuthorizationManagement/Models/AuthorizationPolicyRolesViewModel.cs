using Camino.Framework.Models;
using System.Collections.Generic;

namespace Module.Web.AuthorizationManagement.Models
{
    public class AuthorizationPolicyRolesViewModel : PageListViewModel<RoleViewModel>
    {
        public AuthorizationPolicyRolesViewModel(IEnumerable<RoleViewModel> collections) : base(collections)
        {
            Filter = new RoleAuthorizationPolicyFilterViewModel();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte RoleId { get; set; }
    }
}
