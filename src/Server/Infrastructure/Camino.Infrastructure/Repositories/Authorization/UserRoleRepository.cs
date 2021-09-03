using Camino.Core.Contracts.Data;
using Camino.Shared.Results.Identifiers;
using LinqToDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Authorization;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Results.Authorization;
using Camino.Shared.Requests.Authorization;

namespace Camino.Infrastructure.Repositories.Authorization
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IRepository<UserRole> _userRoleRepository;

        public UserRoleRepository(IRepository<UserRole> userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<IList<UserRoleResult>> GetUserRolesAsync(long userId)
        {
            var userRoles = await _userRoleRepository.Get(x => x.UserId == userId)
                .Select(x => new UserRoleResult()
                {
                    RoleId = x.RoleId,
                    RoleName = x.Role.Name,
                    UserId = x.UserId
                }).ToListAsync();

            return userRoles;
        }

        public async Task<UserRoleResult> FindUserRoleAsync(long userId, long roleId)
        {
            var userRole = await _userRoleRepository.Get(x => x.UserId == userId && x.RoleId == roleId)
                .Select(x => new UserRoleResult
                {
                    RoleId = x.RoleId,
                    UserId = x.UserId,
                    RoleName = x.Role.Name
                })
                .FirstOrDefaultAsync();

            return userRole;
        }

        public async Task<IList<UserResult>> GetUsersInRoleAsync(long roleId)
        {
            var existUserRoles = await _userRoleRepository.Get(x => x.RoleId == roleId)
                .Select(x => new UserResult()
                {
                    Id = x.UserId,
                    DisplayName = x.User.DisplayName,
                    Lastname = x.User.Lastname,
                    Firstname = x.User.Firstname,
                    UserName = x.User.UserName,
                    Email = x.User.Email,
                    IsEmailConfirmed = x.User.IsEmailConfirmed,
                    StatusId = x.User.StatusId
                }).ToListAsync();

            return existUserRoles;
        }

        public void Create(UserRoleRequest request)
        {
            var userRole = new UserRole
            {
                RoleId = request.RoleId,
                UserId = request.UserId
            };
            _userRoleRepository.Add(userRole);
        }

        public void Remove(UserRoleRequest request)
        {
            var userRole = new UserRole
            {
                RoleId = request.RoleId,
                UserId = request.UserId
            };
            _userRoleRepository.Delete(userRole);
        }
    }
}
