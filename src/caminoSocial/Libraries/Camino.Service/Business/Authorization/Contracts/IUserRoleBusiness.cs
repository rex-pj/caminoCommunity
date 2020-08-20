using Camino.Service.Data.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authorization.Contracts
{
    public interface IUserRoleBusiness
    {
        Task<IList<UserRoleResult>> GetUserRolesAsync(long userId);
        Task<UserRoleResult> FindUserRoleAsync(long userId, long roleId);
        Task<IList<UserResult>> GetUsersInRoleAsync(long roleId);
        void Remove(UserRoleResult userRoleDto);
        void Add(UserRoleResult userRoleDto);
    }
}
