using System;
using System.Threading.Tasks;
using Camino.Core.Domains.Authorization.Repositories;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains;
using Camino.Core.Domains.Authorization;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authorization
{
    public class RoleRepository : IRoleRepository, IScopedDependency
    {
        private readonly IEntityRepository<Role> _roleEntityRepository;
        private readonly IAppDbContext _dbContext;

        public RoleRepository(IEntityRepository<Role> roleEntityRepository, IAppDbContext dbContext)
        {
            _roleEntityRepository = roleEntityRepository;
            _dbContext = dbContext;
        }

        #region CRUD
        public async Task<long> CreateAsync(Role role)
        {
            var modifiedDate = DateTime.UtcNow;
            role.CreatedDate = modifiedDate;
            role.UpdatedDate = modifiedDate;

            await _roleEntityRepository.InsertAsync(role);
            await _dbContext.SaveChangesAsync();
            return role.Id;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await _roleEntityRepository.DeleteAsync(x => x.Id == id);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<Role> FindAsync(long id)
        {
            var existRole = await _roleEntityRepository.FindAsync(x => x.Id == id);
            return existRole;
        }

        public async Task<Role> FindByNameAsync(string name)
        {
            name = name.ToLower();
            var role = await _roleEntityRepository.FindAsync(x => x.Name.ToLower() == name);
            return role;
        }

        public async Task<bool> UpdateAsync(Role role)
        {
            role.UpdatedDate = DateTime.UtcNow;
            await _roleEntityRepository.UpdateAsync(role);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
