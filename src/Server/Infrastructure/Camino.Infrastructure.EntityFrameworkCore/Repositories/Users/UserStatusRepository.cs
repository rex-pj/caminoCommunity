using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Users;
using Camino.Core.Domains.Users.Repositories;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Users
{
    public class UserStatusRepository : IUserStatusRepository, IScopedDependency
    {
        private readonly IEntityRepository<Status> _statusRepository;
        private readonly IDbContext _dbContext;

        public UserStatusRepository(IEntityRepository<Status> statusRepository, IDbContext dbContext)
        {
            _statusRepository = statusRepository;
            _dbContext = dbContext;
        }

        public Status Find(int id)
        {
            var status = _statusRepository.Get(x => x.Id == id)
                .FirstOrDefault();

            return status;
        }

        public Status FindByName(string name)
        {
            var status = _statusRepository.Get(x => x.Name == name)
                .FirstOrDefault();

            return status;
        }

        public async Task<int> CreateAsync(Status status)
        {
            await _statusRepository.InsertAsync(status);
            await _dbContext.SaveChangesAsync();
            return status.Id;
        }

        public async Task<bool> UpdateAsync(Status status)
        {
            await _statusRepository.UpdateAsync(status);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
