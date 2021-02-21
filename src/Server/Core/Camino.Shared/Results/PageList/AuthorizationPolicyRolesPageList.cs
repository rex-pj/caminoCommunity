using Camino.Shared.Results.Authorization;
using System.Collections.Generic;

namespace Camino.Shared.Results.PageList
{
    public class AuthorizationPolicyRolesPageList : BasePageList<RoleResult>
    {
        public AuthorizationPolicyRolesPageList(IEnumerable<RoleResult> collections) : base(collections)
        {

        }

        public AuthorizationPolicyRolesPageList() : base(new List<RoleResult>())
        {

        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
