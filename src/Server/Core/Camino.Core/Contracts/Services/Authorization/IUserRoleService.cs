using Camino.Shared.Requests.Authorization;
using Camino.Shared.Results.Authorization;
using Camino.Shared.Results.Identifiers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Services.Authorization
{
    public interface IUserRoleService
    {
        Task<IList<UserRoleResult>> GetUserRolesAsync(long userId);
        Task<UserRoleResult> FindUserRoleAsync(long userId, long roleId);
        Task<IList<UserResult>> GetUsersInRoleAsync(long roleId);
        void Remove(UserRoleRequest request);
        void Create(UserRoleRequest request);
    }
}
