using Camino.Infrastructure.AspNetCore.Models;
using System.Collections.Generic;

namespace Module.Authorization.WebAdmin.Models
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
