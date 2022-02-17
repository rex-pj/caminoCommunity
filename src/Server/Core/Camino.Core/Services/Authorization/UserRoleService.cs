using Camino.Shared.Results.Identifiers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Core.Contracts.Services.Authorization;
using Camino.Shared.Results.Authorization;
using Camino.Core.Contracts.Repositories.Authorization;
using Camino.Shared.Requests.Authorization;
using Camino.Core.Contracts.DependencyInjection;

namespace Camino.Services.Authorization
{
    public class UserRoleService : IUserRoleService, IScopedDependency
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<IList<UserRoleResult>> GetUserRolesAsync(long userId)
        {
            var userRoles = await _userRoleRepository.GetUserRolesAsync(userId);
            return userRoles;
        }

        public async Task<UserRoleResult> FindUserRoleAsync(long userId, long roleId)
        {
            var userRole = await _userRoleRepository.FindUserRoleAsync(userId, roleId);
            return userRole;
        }

        public async Task<IList<UserResult>> GetUsersInRoleAsync(long roleId)
        {
            var existUserRoles = await _userRoleRepository.GetUsersInRoleAsync(roleId);
            return existUserRoles;
        }

        public void Create(UserRoleRequest request)
        {
            _userRoleRepository.Create(request);
        }

        public void Remove(UserRoleRequest request)
        {
            _userRoleRepository.Remove(request);
        }
    }
}
