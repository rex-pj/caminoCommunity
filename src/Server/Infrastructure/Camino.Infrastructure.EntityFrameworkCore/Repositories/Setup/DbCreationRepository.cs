using Camino.Core.Contracts.Repositories.Setup;
using Camino.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Setup
{
    public class DbCreationRepository : IDbCreationRepository, IScopedDependency
    {
        private readonly IAppDbContext _dbContext;
        public DbCreationRepository(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsDatabaseExist()
        {
            var canConnect = _dbContext.Database.CanConnect();
            if (!canConnect)
            {
                return false;
            }

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
