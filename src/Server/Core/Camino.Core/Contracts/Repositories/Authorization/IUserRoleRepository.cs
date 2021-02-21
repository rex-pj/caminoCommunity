using Camino.Shared.Requests.Authorization;
using Camino.Shared.Results.Authorization;
using Camino.Shared.Results.Identifiers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Authorization
{
    public interface IUserRoleRepository
    {
        Task<IList<UserRoleResult>> GetUserRolesAsync(long userId);
        Task<UserRoleResult> FindUserRoleAsync(long userId, long roleId);
        Task<IList<UserResult>> GetUsersInRoleAsync(long roleId);
        void Remove(UserRoleRequest userRoleRequest);
        void Create(UserRoleRequest userRoleRequest);
    }
}
