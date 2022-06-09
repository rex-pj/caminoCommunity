using Camino.Core.Contracts.Repositories.Users;
using Camino.Application.Contracts.AppServices.Users;
using Camino.Core.DependencyInjection;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Core.Domains.Users;
using Camino.Core.Domains.Users.DomainServices;

namespace Camino.Application.AppServices.Users
{
    public class UserAttributeAppService : IUserAttributeAppService, IScopedDependency
    {
        private readonly IUserAttributeRepository _userAttributeRepository;
        private readonly IUserAttributeDomainService _userAttributeDomainService;

        public UserAttributeAppService(IUserAttributeRepository userAttributeRepository,
            IUserAttributeDomainService userAttributeDomainService)
        {
            _userAttributeRepository = userAttributeRepository;
            _userAttributeDomainService = userAttributeDomainService;
        }

        public async Task<UserAttributeResult> GetAsync(long userId, string key)
        {
            var existing = await _userAttributeRepository.GetAsync(userId, key);
            if (existing == null)
            {
                return null;
            }

            var data = MapEntityToDto(existing);
            return data;
        }

        public async Task<IEnumerable<UserAttributeResult>> GetAsync(long userId)
        {
            var exists = (await _userAttributeRepository.GetAsync(userId))
                .Select(x => new UserAttributeResult()
                {
                    Expiration = x.Expiration,
                    Id = x.Id,
                    IsDisabled = x.IsDisabled,
                    Key = x.Key,
                    UserId = x.UserId,
                    Value = x.Value
                });
            return exists;
        }

        public async Task CreateOrUpdateAsync(IEnumerable<UserAttributeModifyRequest> userAttributes)
        {
            if (userAttributes == null || !userAttributes.Any())
            {
                throw new ArgumentNullException(nameof(userAttributes));
            }

            var mappedUserAttributes = userAttributes.Select(x => new UserAttribute()
            {
                Key = x.Key,
                UserId = x.UserId,
                Value = x.Value,
                Expiration = x.Expiration
            });

            await _userAttributeDomainService.CreateOrUpdateAsync(mappedUserAttributes);
        }

        public async Task<int> CreateOrUpdateAsync(long userId, string key, string value, DateTime? expiration = null)
        {
            return await _userAttributeDomainService.CreateOrUpdateAsync(userId, key, value, expiration);
        }

        public async Task<bool> DeleteAsync(long userId, string key, string value)
        {
            return await _userAttributeRepository.DeleteAsync(userId, key, value);
        }

        public async Task<bool> DeleteAsync(long userId, string key)
        {
            return await _userAttributeRepository.DeleteAsync(userId, key);
        }

        private UserAttributeResult MapEntityToDto(UserAttribute entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new UserAttributeResult
            {
                Expiration = entity.Expiration,
                Id = entity.Id,
                IsDisabled = entity.IsDisabled,
                Key = entity.Key,
                UserId = entity.UserId,
                Value = entity.Value
            };
        }
    }
}
