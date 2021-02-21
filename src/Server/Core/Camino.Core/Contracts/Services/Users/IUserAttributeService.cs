using Camino.Shared.Requests.Identifiers;
using Camino.Shared.Results.Identifiers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Services.Users
{
    public interface IUserAttributeService
    {
        IEnumerable<int> Create(IEnumerable<UserAttributeModifyRequest> attributes);
        Task<UserAttributeResult> GetAsync(long userId, string key);
        Task<IEnumerable<UserAttributeResult>> GetAsync(long userId);
        Task<int> CreateOrUpdateAsync(long userId, string key, string value, DateTime? expiration = null);
        Task<IEnumerable<int>> CreateOrUpdateAsync(IEnumerable<UserAttributeModifyRequest> attributes);
        Task<bool> DeleteAsync(long userId, string key, string value);
        Task<bool> DeleteAsync(long userId, string key);
    }
}
