using Camino.Data.Contracts;
using Camino.Service.Projections.Identity;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Authentication.Contracts;
using Camino.IdentityDAL.Entities;
using LinqToDB;

namespace Camino.Service.Business.Authentication
{
    public class UserTokenBusiness : IUserTokenBusiness
    {
        private readonly IRepository<UserToken> _userTokenRepository;

        public UserTokenBusiness(IRepository<UserToken> userTokenRepository)
        {
            _userTokenRepository = userTokenRepository;
        }

        public async Task<UserTokenProjection> FindAsync(long userId, string loginProvider, string name)
        {
            var userTokens = _userTokenRepository.Get(x => x.UserId == userId && x.Name == name).Select(x => new UserTokenProjection()
            {
                UserId = x.UserId,
                LoginProvider = x.LoginProvider,
                Name = x.Name,
                Value = x.Value
            });

            var userToken = await userTokens.FirstOrDefaultAsync();
            return userToken;
        }

        public void Create(UserTokenProjection userTokenRequest)
        {
            var userToken = new UserToken()
            {
                LoginProvider = userTokenRequest.LoginProvider,
                Name = userTokenRequest.Name,
                Value = userTokenRequest.Value,
                UserId = userTokenRequest.UserId
            };
            _userTokenRepository.Add(userToken);
        }

        public void Remove(UserTokenProjection userTokenRequest)
        {
            var userToken = _userTokenRepository.Get(x => x.LoginProvider == userTokenRequest.LoginProvider
                && x.Value == userTokenRequest.Value && x.Name == userTokenRequest.Name && x.UserId == userTokenRequest.UserId);
            _userTokenRepository.Delete(userToken);
        }
    }
}
