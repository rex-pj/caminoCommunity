using AutoMapper;
using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.Auth;
using Coco.IdentityDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coco.Business.Implementation.UserBusiness
{
    public class UserTokenBusiness : IUserTokenBusiness
    {
        private readonly IRepository<UserToken> _userTokenRepository;
        private readonly IdentityDbContext _identityDbContext;
        private readonly IMapper _mapper;

        public UserTokenBusiness(IRepository<UserToken> userTokenRepository, IdentityDbContext identityDbContext, IMapper mapper)
        {
            _mapper = mapper;
            _userTokenRepository = userTokenRepository;
            _identityDbContext = identityDbContext;
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
            var userToken = _mapper.Map<UserToken>(userTokenDto);
            _userTokenRepository.Delete(userToken);
        }
    }
}
