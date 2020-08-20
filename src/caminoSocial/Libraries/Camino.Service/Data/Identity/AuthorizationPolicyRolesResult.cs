using Camino.Service.Data.Page;
using System.Collections.Generic;

namespace Camino.Service.Data.Identity
{
    public class AuthorizationPolicyRolesResult : PageList<RoleResult>
    {
        public AuthorizationPolicyRolesResult(IEnumerable<RoleResult> collections) : base(collections)
        {

        }

        public AuthorizationPolicyRolesResult() : base(new List<RoleResult>())
        {

        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
