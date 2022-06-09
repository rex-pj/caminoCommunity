namespace Camino.Core.Domains.Users.DomainServices
{
    public interface IUserAttributeDomainService
    {
        Task CreateOrUpdateAsync(IEnumerable<UserAttribute> newUserAttributes);
        Task<int> CreateOrUpdateAsync(long userId, string key, string value, DateTime? expiration = null);
    }
}
