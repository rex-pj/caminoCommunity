using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Core.Domains;
using Camino.Core.Domains.Authorization;
using Camino.Core.Domains.Authorization.Repositories;
using Camino.Core.DependencyInjection;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authorization
{
    public class UserRoleRepository : IUserRoleRepository, IScopedDependency
    {
        private readonly IEntityRepository<UserRole> _userRoleEntityRepository;
        private readonly IAppDbContext _dbContext;

        public UserRoleRepository(IEntityRepository<UserRole> userRoleEntityRepository, IAppDbContext dbContext)
        {
            _userRoleEntityRepository = userRoleEntityRepository;
            _dbContext = dbContext;
        }

        public async Task<IList<UserRole>> GetListAsync(long userId)
        {
            var userRoles = await _userRoleEntityRepository.GetAsync(x => x.UserId == userId);
            return userRoles;
        }

        public async Task<UserRole> FindAsync(long userId, long roleId)
        {
            var userRole = await _userRoleEntityRepository.FindAsync(x => x.UserId == userId && x.RoleId == roleId);
            return userRole;
        }

        public async Task<bool> CreateAsync(UserRole userRole)
        {
            userRole.GrantedDate = DateTime.UtcNow;
            _userRoleEntityRepository.Insert(userRole);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> RemoveAsync(long roleId, long userId)
        {
            var existing = await FindAsync(roleId, userId);
            if (existing == null)
            {
                return false;
            }

            await _userRoleEntityRepository.DeleteAsync(existing);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
