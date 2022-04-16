using Camino.Core.Contracts.Data;
using Camino.Shared.Results.Identifiers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Authorization;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Results.Authorization;
using Camino.Shared.Requests.Authorization;
using Camino.Core.Contracts.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authorization
{
    public class UserRoleRepository : IUserRoleRepository, IScopedDependency
    {
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IAppDbContext _dbContext;

        public UserRoleRepository(IRepository<UserRole> userRoleRepository, IAppDbContext dbContext)
        {
            _userRoleRepository = userRoleRepository;
            _dbContext = dbContext;
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
            _userRoleRepository.Insert(userRole);
            _dbContext.SaveChanges();
        }

        public void Remove(UserRoleRequest request)
        {
            var userRole = new UserRole
            {
                RoleId = request.RoleId,
                UserId = request.UserId
            };
            _userRoleRepository.Delete(userRole);
            _dbContext.SaveChanges();
        }
    }
}
