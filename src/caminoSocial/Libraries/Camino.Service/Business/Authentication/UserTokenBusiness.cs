using AutoMapper;
using Camino.Data.Contracts;
using Camino.Service.Data.Identity;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Authentication.Contracts;
using Camino.IdentityDAL.Entities;

namespace Camino.Service.Business.Authentication
{
    public class UserTokenBusiness : IUserTokenBusiness
    {
        private readonly IRepository<UserToken> _userTokenRepository;
        private readonly IMapper _mapper;

        public UserTokenBusiness(IRepository<UserToken> userTokenRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userTokenRepository = userTokenRepository;
        }

        public async Task<UserTokenResult> FindAsync(long userId, string loginProvider, string name)
        {
            var userTokens = await _userTokenRepository.GetAsync(x => x.UserId == userId && x.Name == name);
            var userToken = userTokens.FirstOrDefault();

            return _mapper.Map<UserTokenResult>(userToken);
        }

        public void Add(UserTokenResult userTokenDto)
        {
            var userToken = _mapper.Map<UserToken>(userTokenDto);
            _userTokenRepository.Add(userToken);
        }

        public void Remove(UserTokenResult userTokenDto)
        {
            var userToken = _userTokenRepository.Get(x => x.LoginProvider == userTokenDto.LoginProvider
                && x.Value == userTokenDto.Value && x.Name == userTokenDto.Name && x.UserId == userTokenDto.UserId);
            _userTokenRepository.Delete(userToken);
        }
    }
}
