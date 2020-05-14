using Coco.Entities.Dtos.User;
using System.Collections.Generic;

namespace Coco.Entities.Dtos.Auth
{
    public class AuthorizationPolicyUsersDto
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<UserDto> AuthorizationPolicyUsers { get; set; }
    }
}
