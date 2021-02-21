using Camino.Shared.Requests.Identifiers;
using Camino.Shared.Results.Identifiers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Users
{
    public interface IUserAttributeRepository
    {
        Task<int> CreateOrUpdateAsync(long userId, string key, string value, DateTime? expiration = null);
        IEnumerable<int> Create(IEnumerable<UserAttributeModifyRequest> attributes);
        Task<UserAttributeResult> GetAsync(long userId, string key);
        Task<IEnumerable<UserAttributeResult>> GetAsync(long userId);
        Task<IEnumerable<int>> CreateOrUpdateAsync(IEnumerable<UserAttributeModifyRequest> userAttributes);
        Task<bool> DeleteAsync(long userId, string key, string value);
        Task<bool> DeleteAsync(long userId, string key);
    }
}
