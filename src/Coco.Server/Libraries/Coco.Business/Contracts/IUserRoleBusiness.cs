using Coco.Core.Dtos.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IUserRoleBusiness
    {
        Task<IList<UserRoleDto>> GetUserRolesAsync(long userId);
        Task<UserRoleDto> FindUserRoleAsync(long userId, long roleId);
        Task<IList<UserDto>> GetUsersInRoleAsync(long roleId);
        void Remove(UserRoleDto userRoleDto);
        void Add(UserRoleDto userRoleDto);
    }
}
