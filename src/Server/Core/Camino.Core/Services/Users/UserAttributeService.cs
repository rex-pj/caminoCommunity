using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Core.Contracts.Services.Users;
using Camino.Shared.Results.Identifiers;
using Camino.Shared.Requests.Identifiers;
using Camino.Core.Contracts.Repositories.Users;

namespace Camino.Services.Users
{
    public class UserAttributeService : IUserAttributeService
    {
        private readonly IUserAttributeRepository _userAttributeRepository;

        public UserAttributeService(IUserAttributeRepository userAttributeRepository)
        {
            _userAttributeRepository = userAttributeRepository;
        }

        public async Task<UserAttributeResult> GetAsync(long userId, string key)
        {
            var exists = await _userAttributeRepository.GetAsync(userId, key);
            return exists;
        }

        public async Task<IEnumerable<UserAttributeResult>> GetAsync(long userId)
        {
            var exists = await _userAttributeRepository.GetAsync(userId);
            return exists;
        }

        public IEnumerable<int> Create(IEnumerable<UserAttributeModifyRequest> attributes)
        {
            var exists = _userAttributeRepository.Create(attributes);
            return exists;
        }

        public async Task<IEnumerable<int>> CreateOrUpdateAsync(IEnumerable<UserAttributeModifyRequest> attributes)
        {
            return await _userAttributeRepository.CreateOrUpdateAsync(attributes);
        }

        public async Task<int> CreateOrUpdateAsync(long userId, string key, string value, DateTime? expiration = null)
        {
            return await _userAttributeRepository.CreateOrUpdateAsync(userId, key, value, expiration);
        }

        public async Task<bool> DeleteAsync(long userId, string key, string value)
        {
            return await _userAttributeRepository.DeleteAsync(userId, key, value);
        }

        public async Task<bool> DeleteAsync(long userId, string key)
        {
            return await _userAttributeRepository.DeleteAsync(userId, key);
        }
    }
}
