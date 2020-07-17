using AutoMapper;
using Camino.Business.Contracts;
using Camino.Data.Contracts;
using Camino.Business.Dtos.Identity;
using Camino.Data.Entities.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Business.Implementation.UserBusiness
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

        public async Task<UserTokenDto> FindAsync(long userId, string loginProvider, string name)
        {
            var userTokens = await _userTokenRepository.GetAsync(x => x.UserId == userId && x.Name == name);
            var userToken = userTokens.FirstOrDefault();

            return _mapper.Map<UserTokenDto>(userToken);
        }

        public void Add(UserTokenDto userTokenDto)
        {
            var userToken = _mapper.Map<UserToken>(userTokenDto);
            _userTokenRepository.Add(userToken);
        }

        public void Remove(UserTokenDto userTokenDto)
        {
            var userToken = _userTokenRepository.Get(x => x.LoginProvider == userTokenDto.LoginProvider
                && x.Value == userTokenDto.Value && x.Name == userTokenDto.Name && x.UserId == userTokenDto.UserId);
            _userTokenRepository.Delete(userToken);
        }
    }
}
