using Camino.Core.Contracts.Data;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Camino.Core.Contracts.Repositories.Authentication;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Results.Authentication;
using Camino.Shared.Requests.Authentication;

namespace Camino.Service.Repository.Authentication
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly IRepository<UserToken> _userTokenRepository;

        public UserTokenRepository(IRepository<UserToken> userTokenRepository)
        {
            _userTokenRepository = userTokenRepository;
        }

        public async Task<UserTokenResult> FindAsync(long userId, string loginProvider, string name)
        {
            var userTokens = _userTokenRepository.Get(x => x.UserId == userId && x.Name == name).Select(x => new UserTokenResult()
            {
                UserId = x.UserId,
                LoginProvider = x.LoginProvider,
                Name = x.Name,
                Value = x.Value
            });

            var userToken = await userTokens.FirstOrDefaultAsync();
            return userToken;
        }

        public void Create(UserTokenRequest request)
        {
            var userToken = new UserToken()
            {
                LoginProvider = request.LoginProvider,
                Name = request.Name,
                Value = request.Value,
                UserId = request.UserId
            };
            _userTokenRepository.Add(userToken);
        }

        public void Remove(UserTokenRequest request)
        {
            var userToken = _userTokenRepository.Get(x => x.LoginProvider == request.LoginProvider
                && x.Value == request.Value && x.Name == request.Name && x.UserId == request.UserId);
            _userTokenRepository.Delete(userToken);
        }
    }
}
