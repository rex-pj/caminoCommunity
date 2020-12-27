using Camino.Service.Projections.Identity;
using System.Collections.Generic;

namespace Camino.Service.Projections.PageList
{
    public class AuthorizationPolicyRolesPageList : BasePageList<RoleProjection>
    {
        public AuthorizationPolicyRolesPageList(IEnumerable<RoleProjection> collections) : base(collections)
        {

        }

        public AuthorizationPolicyRolesPageList() : base(new List<RoleProjection>())
        {

        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
