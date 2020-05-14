using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Entities.Domain.Auth;
using Coco.Entities.Dtos.Auth;
using Coco.Entities.Dtos.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation
{
    public class UserRoleBusniess : IUserRoleBusniess
    {
        private readonly IRepository<UserRole> _userRoleRepository;
        public UserRoleBusniess(IRepository<UserRole> userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<List<UserRoleDto>> GetUserRolesAsync(UserDto user)
        {
            var userRoles = (await _userRoleRepository.GetAsync(x => x.UserId == user.Id))
                .Select(x => new UserRoleDto()
                {
                    RoleId = x.RoleId,
                    RoleName = x.Role.Name,
                    UserId = x.UserId
                }).ToList();

            return userRoles;
        }
    }
}
