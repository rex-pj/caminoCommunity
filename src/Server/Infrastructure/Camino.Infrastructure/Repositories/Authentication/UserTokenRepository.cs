using Camino.Core.Contracts.Data;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Camino.Core.Contracts.Repositories.Authentication;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Results.Authentication;
using Camino.Shared.Requests.Authentication;
using Camino.Core.Contracts.DependencyInjection;

namespace Camino.Infrastructure.Repositories.Authentication
{
    public class UserTokenRepository : IUserTokenRepository, IScopedDependency
    {
        private readonly IEntityRepository<UserToken> _userTokenRepository;

        public UserTokenRepository(IEntityRepository<UserToken> userTokenRepository)
        {
            _userTokenRepository = userTokenRepository;
        }

        public async Task<UserTokenResult> FindAsync(long userId, string loginProvider, string name)
        {
            var userTokens = _userTokenRepository.Get(x => x.UserId == userId && x.Name == name).Select(x => new UserTokenResult
            {
                Id = x.Id,
                UserId = x.UserId,
                LoginProvider = x.LoginProvider,
                Name = x.Name,
                Value = x.Value,
                ExpiryTime = x.ExpiryTime
            });

            var userToken = await userTokens.FirstOrDefaultAsync();
            return userToken;
        }

        public async Task<UserTokenResult> FindByValueAsync(long userId, string value, string name)
        {
            var userTokens = _userTokenRepository.Get(x => x.UserId == userId && x.Name == name && x.Value == value)
                .Select(x => new UserTokenResult
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    LoginProvider = x.LoginProvider,
                    Name = x.Name,
                    Value = x.Value,
                    ExpiryTime = x.ExpiryTime
                });

            var userToken = await userTokens.FirstOrDefaultAsync();
            return userToken;
        }

        public void Create(UserTokenRequest request)
        {
            var userToken = new UserToken
            {
                LoginProvider = request.LoginProvider,
                Name = request.Name,
                Value = request.Value,
                UserId = request.UserId,
                ExpiryTime = request.ExpiryTime
            };
            _userTokenRepository.Add(userToken);
        }

        public async Task RemoveAsync(UserTokenRequest request)
        {
            await _userTokenRepository.DeleteAsync(x => x.LoginProvider == request.LoginProvider
                && x.Value == request.Value && x.Name == request.Name && x.UserId == request.UserId);
        }

        public async Task RemoveByValueAsync(UserTokenRequest request)
        {
            await _userTokenRepository.DeleteAsync(x => x.Value == request.Value
                && x.UserId == request.UserId);
        }
    }
}
