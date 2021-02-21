using Camino.Framework.Models;
using System.Collections.Generic;

namespace Module.Web.AuthorizationManagement.Models
{
    public class AuthorizationPolicyUsersModel : PageListModel<UserModel>
    {
        public AuthorizationPolicyUsersModel(IEnumerable<UserModel> collections) : base(collections)
        {
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
