using System.Collections.Generic;

namespace Module.Web.AuthorizationManagement.Models
{
    public class AuthorizationPolicyUsersViewModel
    {
        public AuthorizationPolicyUsersViewModel()
        {
            AuthorizationPolicyUsers = new List<UserViewModel>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }

        public IEnumerable<UserViewModel> AuthorizationPolicyUsers { get; set; }
    }
}
