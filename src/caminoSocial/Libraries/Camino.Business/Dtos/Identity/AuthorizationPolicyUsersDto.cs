using Camino.Business.Dtos.General;
using System.Collections.Generic;

namespace Camino.Business.Dtos.Identity
{
    public class AuthorizationPolicyUsersDto : PageListDto<UserDto>
    {
        public AuthorizationPolicyUsersDto(IEnumerable<UserDto> collections) : base(collections)
        {

        }

        public AuthorizationPolicyUsersDto() : base(new List<UserDto>())
        {

        }


        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
