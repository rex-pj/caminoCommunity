using AutoMapper;
using Camino.Data.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Authentication.Contracts;
using Camino.IdentityDAL.Entities;
using Camino.Service.Data.Request;

namespace Camino.Service.Business.Authentication
{
    public class UserLoginBusiness : IUserLoginBusiness
    {
        private readonly IRepository<UserLogin> _userLoginRepository;
        private readonly IMapper _mapper;

        public UserLoginBusiness(IRepository<UserLogin> userTokenRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userLoginRepository = userTokenRepository;
        }

        public async Task<UserLoginRequest> FindAsync(long userId, string loginProvider, string providerKey)
        {
            var userLogins = await _userLoginRepository.GetAsync(x => x.UserId == userId && x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
            var userLogin = userLogins.FirstOrDefault();

            return _mapper.Map<UserLoginRequest>(userLogin);
        }

        public async Task<UserLoginRequest> FindAsync(string loginProvider, string providerKey)
        {
            var userLogins = await _userLoginRepository.GetAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
            var userLogin = userLogins.FirstOrDefault();

            return _mapper.Map<UserLoginRequest>(userLogin);
        }

        public async Task<IList<UserLoginRequest>> GetByUserIdAsync(long userId)
        {
            var userLogins = await _userLoginRepository.GetAsync(x => x.UserId == userId);
            return _mapper.Map<IList<UserLoginRequest>>(userLogins);
        }

        public void Add(UserLoginRequest userLoginRequest)
        {
            var userLogin = _mapper.Map<UserLogin>(userLoginRequest);
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
