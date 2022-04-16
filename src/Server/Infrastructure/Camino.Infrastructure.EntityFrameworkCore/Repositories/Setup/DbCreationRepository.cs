using Camino.Core.Contracts.DependencyInjection;
using Camino.Core.Contracts.Repositories.Setup;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Setup
{
    public class DbCreationRepository : IDbCreationRepository, IScopedDependency
    {
        private readonly CaminoDbContext _dbContext;
        public DbCreationRepository(CaminoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsDatabaseExist()
        {
            return _dbContext.Database.GetService<IRelationalDatabaseCreator>().Exists();
        }

        public async Task CreateDatabaseAsync()
        {
            if (IsDatabaseExist())
            {
                return;
            }

            await _dbContext.Database.EnsureCreatedAsync();
        }
    }
}
