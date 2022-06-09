using Camino.Application.Contracts.AppServices.Users.Dtos;

namespace Camino.Application.Contracts.AppServices.Users
{
    public interface IUserAttributeAppService
    {
        Task CreateOrUpdateAsync(IEnumerable<UserAttributeModifyRequest> userAttributes);
        Task<int> CreateOrUpdateAsync(long userId, string key, string value, DateTime? expiration = null);
        Task<bool> DeleteAsync(long userId, string key);
        Task<bool> DeleteAsync(long userId, string key, string value);
        Task<IEnumerable<UserAttributeResult>> GetAsync(long userId);
        Task<UserAttributeResult> GetAsync(long userId, string key);
    }
}
