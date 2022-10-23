using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Core.Domains.Authentication.Repositories;
using Camino.Core.Domains;
using Camino.Core.Domains.Authentication;
using Camino.Core.DependencyInjection;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authentication
{
    public class UserLoginRepository : IUserLoginRepository, IScopedDependency
    {
        private readonly IEntityRepository<UserLogin> _userLoginRepository;
        private readonly IDbContext _dbContext;

        public UserLoginRepository(IEntityRepository<UserLogin> userTokenRepository, IDbContext dbContext)
        {
            _userLoginRepository = userTokenRepository;
            _dbContext = dbContext;
        }

        public async Task<UserLogin> FindAsync(long userId, string loginProvider, string providerKey)
        {
            var userLogin = await _userLoginRepository
                .FindAsync(x => x.UserId == userId && x.LoginProvider == loginProvider && x.ProviderKey == providerKey);

            return userLogin;
        }

        public async Task<UserLogin> FindAsync(string loginProvider, string providerKey)
        {
            var userLogin = await _userLoginRepository
                .FindAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);

            return userLogin;
        }

        public async Task<IList<UserLogin>> GetByUserIdAsync(long userId)
        {
            var userLogins = await _userLoginRepository.GetAsync(x => x.UserId == userId);
            return userLogins;
        }

        public async Task<long> CreateAsync(UserLogin userLogin)
        {
            _userLoginRepository.Insert(userLogin);
            await _dbContext.SaveChangesAsync();
            return userLogin.Id;
        }

        public async Task<bool> RemoveAsync(string loginProvider, string providerKey, string providerDisplayName, long userId)
        {
            await _userLoginRepository.DeleteAsync(x => x.LoginProvider == loginProvider
                    && x.ProviderKey == providerKey
                    && x.ProviderDisplayName == providerDisplayName
                    && x.UserId == userId);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
