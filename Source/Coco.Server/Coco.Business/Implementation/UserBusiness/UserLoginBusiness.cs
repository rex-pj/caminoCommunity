using AutoMapper;
using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.Auth;
using Coco.IdentityDAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation.UserBusiness
{
    public class UserLoginBusiness : IUserLoginBusiness
    {
        private readonly IRepository<UserLogin> _userLoginRepository;
        //private readonly IdentityDbConnection _identityDbContext;
        private readonly IMapper _mapper;

        public UserLoginBusiness(IRepository<UserLogin> userTokenRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userLoginRepository = userTokenRepository;
            //_identityDbContext = identityDbContext;
        }

        public async Task<UserLoginDto> FindAsync(long userId, string loginProvider, string providerKey)
        {
            var userLogins = await _userLoginRepository.GetAsync(x => x.UserId == userId && x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
            var userLogin = userLogins.FirstOrDefault();

            return _mapper.Map<UserLoginDto>(userLogin);
        }

        public async Task<UserLoginDto> FindAsync(string loginProvider, string providerKey)
        {
            var userLogins = await _userLoginRepository.GetAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
            var userLogin = userLogins.FirstOrDefault();

            return _mapper.Map<UserLoginDto>(userLogin);
        }

        public async Task<IList<UserLoginDto>> GetByUserIdAsync(long userId)
        {
            var userLogins = await _userLoginRepository.GetAsync(x => x.UserId == userId);
            return _mapper.Map<IList<UserLoginDto>>(userLogins);
        }

        public void Add(UserLoginDto userLoginDto)
        {
            var userLogin = _mapper.Map<UserLogin>(userLoginDto);
            _userLoginRepository.Add(userLogin);
        }

        public void Remove(UserLoginDto userLoginDto)
        {
            var userLogin = _userLoginRepository
                .Get(x => x.LoginProvider == userLoginDto.LoginProvider && x.ProviderKey == userLoginDto.ProviderKey 
                && x.ProviderDisplayName == userLoginDto.ProviderDisplayName && x.UserId == userLoginDto.UserId);

            _userLoginRepository.Delete(userLogin);
        }
    }
}
