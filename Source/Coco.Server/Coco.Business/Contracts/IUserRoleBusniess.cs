using Coco.Entities.Dtos.Auth;
using Coco.Entities.Dtos.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IUserRoleBusniess
    {
        Task<List<UserRoleDto>> GetUserRolesAsync(UserDto user);
    }
}
