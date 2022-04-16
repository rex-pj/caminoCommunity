using Camino.Core.Contracts.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Authentication;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Requests.Authentication;
using Camino.Shared.Results.Authentication;
using Camino.Core.Contracts.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authentication
{
    public class UserLoginRepository : IUserLoginRepository, IScopedDependency
    {
        private readonly IEntityRepository<UserLogin> _userLoginRepository;
        private readonly IAppDbContext _dbContext;

        public UserLoginRepository(IEntityRepository<UserLogin> userTokenRepository, IAppDbContext dbContext)
        {
            _userLoginRepository = userTokenRepository;
            _dbContext = dbContext;
        }

        public async Task<UserLoginResult> FindAsync(long userId, string loginProvider, string providerKey)
        {
            var userLogins = _userLoginRepository
                .Get(x => x.UserId == userId && x.LoginProvider == loginProvider && x.ProviderKey == providerKey)
                .Select(x => new UserLoginResult()
                {
                    Id = x.Id,
                    LoginProvider = x.LoginProvider,
                    ProviderDisplayName = x.ProviderDisplayName,
                    ProviderKey = x.ProviderKey,
                    UserId = x.UserId
                });

            var userLogin = await userLogins.FirstOrDefaultAsync();
            if (userLogin == null)
            {
                return new UserLoginResult();
            }

            return userLogin;
        }

        public async Task<UserLoginResult> FindAsync(string loginProvider, string providerKey)
        {
            var userLogins = _userLoginRepository.Get(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey)
                .Select(x => new UserLoginResult()
                {
                    Id = x.Id,
                    LoginProvider = x.LoginProvider,
                    ProviderDisplayName = x.ProviderDisplayName,
                    ProviderKey = x.ProviderKey,
                    UserId = x.UserId
                });

            var userLogin = await userLogins.FirstOrDefaultAsync();
            if (userLogin == null)
            {
                return new UserLoginResult();
            }

            return userLogin;
        }

        public async Task<IList<UserLoginResult>> GetByUserIdAsync(long userId)
        {
            var userLogins = await _userLoginRepository.Get(x => x.UserId == userId)
                .Select(x => new UserLoginResult()
                {
                    Id = x.Id,
                    LoginProvider = x.LoginProvider,
                    ProviderDisplayName = x.ProviderDisplayName,
                    ProviderKey = x.ProviderKey,
                    UserId = x.UserId
                }).ToListAsync();
            return userLogins;
        }

        public void Create(UserLoginRequest userLoginRequest)
        {
            var userLogin = new UserLogin()
            {
                LoginProvider = userLoginRequest.LoginProvider,
                ProviderDisplayName = userLoginRequest.ProviderDisplayName,
                ProviderKey = userLoginRequest.ProviderKey,
                UserId = userLoginRequest.UserId
            };
            _userLoginRepository.Insert(userLogin);
            _dbContext.SaveChanges();
        }

        public async Task RemoveAsync(UserLoginRequest userLoginRequest)
        {
            await _userLoginRepository.DeleteAsync(x => x.LoginProvider == userLoginRequest.LoginProvider
                    && x.ProviderKey == userLoginRequest.ProviderKey
                    && x.ProviderDisplayName == userLoginRequest.ProviderDisplayName
                    && x.UserId == userLoginRequest.UserId);
            await _dbContext.SaveChangesAsync();
        }
    }
}
