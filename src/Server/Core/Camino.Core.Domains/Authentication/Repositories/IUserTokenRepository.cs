namespace Camino.Core.Domains.Authentication.Repositories
{
    public interface IUserTokenRepository
    {
        Task<long> CreateAsync(UserToken userToken);
        Task<UserToken> FindAsync(long userId, string loginProvider, string name);
        Task<UserToken> FindByValueAsync(long userId, string value, string name);
        Task RemoveAsync(string loginProvider, string value, string name, long userId);
        Task RemoveByValueAsync(string value, long userId);
    }
}
