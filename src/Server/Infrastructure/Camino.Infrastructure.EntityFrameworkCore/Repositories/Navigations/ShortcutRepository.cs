using Camino.Core.Domains.Navigations;
using System.Threading.Tasks;
using System;
using Camino.Core.Contracts.Repositories.Navigations;
using Microsoft.EntityFrameworkCore;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains;
using Camino.Core.Domains.Farms;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Navigations
{
    public class ShortcutRepository : IShortcutRepository, IScopedDependency
    {
        private readonly IEntityRepository<Shortcut> _shortcutRepository;
        private readonly IDbContext _dbContext;

        public ShortcutRepository(IEntityRepository<Shortcut> shortcutRepository, IDbContext dbContext)
        {
            _shortcutRepository = shortcutRepository;
            _dbContext = dbContext;
        }

        public async Task<Shortcut> FindAsync(int id)
        {
            var shortcut = await _shortcutRepository.FindAsync(x => x.Id == id);
            return shortcut;
        }

        public async Task<Shortcut> FindByNameAsync(string name)
        {
            var shortcut = await _shortcutRepository.Get(x => x.Name == name).FirstOrDefaultAsync();
            return shortcut;
        }

        public async Task<int> CreateAsync(Shortcut shortcut)
        {
            await _shortcutRepository.InsertAsync(shortcut);
            await _dbContext.SaveChangesAsync();
            return shortcut.Id;
        }

        public async Task<bool> UpdateAsync(Shortcut shortcut)
        {
            shortcut.UpdatedDate = DateTime.UtcNow;
            await _shortcutRepository.UpdateAsync(shortcut);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedNumbers = await _shortcutRepository.DeleteAsync(x => x.Id == id);
            await _dbContext.SaveChangesAsync();
            return deletedNumbers > 0;
        }
    }
}
