using Camino.Data.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Authentication.Contracts;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.Request;
using LinqToDB;

namespace Camino.Service.Business.Authentication
{
    public class UserLoginBusiness : IUserLoginBusiness
    {
        private readonly IRepository<UserLogin> _userLoginRepository;

        public UserLoginBusiness(IRepository<UserLogin> userTokenRepository)
        {
            _userLoginRepository = userTokenRepository;
        }

        public async Task<UserLoginRequest> FindAsync(long userId, string loginProvider, string providerKey)
        {
            var userLogins = _userLoginRepository
                .Get(x => x.UserId == userId && x.LoginProvider == loginProvider && x.ProviderKey == providerKey)
                .Select(x => new UserLoginRequest()
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
                return new UserLoginRequest();
            }

            return userLogin;
        }

        public async Task<UserLoginRequest> FindAsync(string loginProvider, string providerKey)
        {
            var userLogins = _userLoginRepository.Get(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey)
                .Select(x => new UserLoginRequest()
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
                return new UserLoginRequest();
            }

            return userLogin;
        }

        public async Task<IList<UserLoginRequest>> GetByUserIdAsync(long userId)
        {
            var userLogins = await _userLoginRepository.Get(x => x.UserId == userId)
                .Select(x => new UserLoginRequest()
                {
                    Id = x.Id,
                    LoginProvider = x.LoginProvider,
                    ProviderDisplayName = x.ProviderDisplayName,
                    ProviderKey = x.ProviderKey,
                    UserId = x.UserId
                }).ToListAsync();
            return userLogins;
        }

        public void Add(UserLoginRequest userLoginRequest)
        {
            var userLogin = new UserLogin()
            {
                LoginProvider = userLoginRequest.LoginProvider,
                ProviderDisplayName = userLoginRequest.ProviderDisplayName,
                ProviderKey = userLoginRequest.ProviderKey,
                UserId = userLoginRequest.UserId
            };
            _userLoginRepository.Add(userLogin);
        }

        public void Remove(UserLoginRequest userLoginRequest)
        {
            var userLogin = _userLoginRepository
                .Get(x => x.LoginProvider == userLoginRequest.LoginProvider && x.ProviderKey == userLoginRequest.ProviderKey
                && x.ProviderDisplayName == userLoginRequest.ProviderDisplayName && x.UserId == userLoginRequest.UserId);

            _userLoginRepository.Delete(userLogin);
        }
    }
}
