using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Application.Contracts.AppServices.Users.Dtos;

namespace Camino.Application.Contracts.AppServices.Authorization
{
    public interface IUserRoleAppService
    {
        Task<bool> CreateAsync(UserRoleRequest request);
        Task<UserRoleResult> FindUserRoleAsync(long userId, long roleId);
        Task<IList<UserRoleResult>> GetUserRolesAsync(long userId);
        Task<IList<UserResult>> GetUsersInRoleAsync(long roleId);
        Task<bool> RemoveAsync(UserRoleRequest request);
    }
}
