using Camino.Core.Domains.Authentication.Repositories;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains;
using Camino.Core.Domains.Authentication;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authentication
{
    public class UserTokenRepository : IUserTokenRepository, IScopedDependency
    {
        private readonly IEntityRepository<UserToken> _userTokenRepository;
        private readonly IDbContext _dbContext;

        public UserTokenRepository(IEntityRepository<UserToken> userTokenRepository, IDbContext dbContext)
        {
            _userTokenRepository = userTokenRepository;
            _dbContext = dbContext;
        }

        public async Task<UserToken> FindAsync(long userId, string loginProvider, string name)
        {
            var userToken = await _userTokenRepository.FindAsync(x => x.UserId == userId && x.Name == name);
            return userToken;
        }

        public async Task<UserToken> FindByValueAsync(long userId, string value, string name)
        {
            var userToken = await _userTokenRepository.FindAsync(x => x.UserId == userId && x.Name == name && x.Value == value);
            return userToken;
        }

        public async Task<long> CreateAsync(UserToken userToken)
        {
            _userTokenRepository.Insert(userToken);
            await _dbContext.SaveChangesAsync();

            return userToken.Id;
        }

        public async Task RemoveAsync(string loginProvider, string value, string name, long userId)
        {
            await _userTokenRepository.DeleteAsync(x => x.LoginProvider == loginProvider
                && x.Value == value && x.Name == name && x.UserId == userId);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveByValueAsync(string value, long userId)
        {
            await _userTokenRepository.DeleteAsync(x => x.Value == value
                && x.UserId == userId);

            await _dbContext.SaveChangesAsync();
        }
    }
}
