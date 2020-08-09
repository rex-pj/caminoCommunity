using Camino.Business.Dtos.General;
using System.Collections.Generic;

namespace Camino.Business.Dtos.Identity
{
    public class AuthorizationPolicyRolesDto : PageListDto<RoleDto>
    {
        public AuthorizationPolicyRolesDto(IEnumerable<RoleDto> collections) : base(collections)
        {

        }

        public AuthorizationPolicyRolesDto() : base(new List<RoleDto>())
        {

        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
