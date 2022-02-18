using Camino.Core.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Results.Identifiers;
using LinqToDB;
using Camino.Shared.Requests.Identifiers;
using Camino.Infrastructure.Linq2Db;
using LinqToDB.Tools;
using Camino.Core.Contracts.DependencyInjection;

namespace Camino.Infrastructure.Repositories.Users
{
    public class UserAttributeRepository : IUserAttributeRepository, IScopedDependency
    {
        private readonly IEntityRepository<UserAttribute> _userAttributeRepository;
        private readonly CaminoDataConnection _dataConnection;

        public UserAttributeRepository(IEntityRepository<UserAttribute> userAttributeRepository, CaminoDataConnection dataConnection)
        {
            _userAttributeRepository = userAttributeRepository;
            _dataConnection = dataConnection;
        }

        public async Task<UserAttributeResult> GetAsync(long userId, string key)
        {
            var exists = _userAttributeRepository.Get(x => x.UserId == userId && x.Key.Equals(key))
                .Select(x => new UserAttributeResult()
                {
                    Expiration = x.Expiration,
                    Id = x.Id,
                    IsDisabled = x.IsDisabled,
                    Key = x.Key,
                    UserId = x.UserId,
                    Value = x.Value
                });
            if (exists == null || !exists.Any())
            {
                return null;
            }

            var data = await exists.FirstOrDefaultAsync();
            return data;
        }

        public async Task<IEnumerable<UserAttributeResult>> GetAsync(long userId)
        {
            var exists = _userAttributeRepository.Get(x => x.UserId == userId)
                .Select(x => new UserAttributeResult()
                {
                    Expiration = x.Expiration,
                    Id = x.Id,
                    IsDisabled = x.IsDisabled,
                    Key = x.Key,
                    UserId = x.UserId,
                    Value = x.Value
                });
            return await exists.ToListAsync();
        }

        public IEnumerable<int> Create(IEnumerable<UserAttributeModifyRequest> attributes)
        {
            if (attributes == null || !attributes.Any())
            {
                return new List<int>();
            }

            var userAttributes = new List<UserAttribute>();

            foreach (var item in userAttributes)
            {
                var attribute = new UserAttribute()
                {
                    Key = item.Key,
                    UserId = item.UserId,
                    Value = item.Value,
                    Expiration = item.Expiration
                };

                userAttributes.Add(attribute);
            }

            _userAttributeRepository.Add(userAttributes);
            return userAttributes.Select(x => x.Id);
        }

        public async Task<IEnumerable<int>> CreateOrUpdateAsync(IEnumerable<UserAttributeModifyRequest> userAttributes)
        {
            if (userAttributes == null || !userAttributes.Any())
            {
                return null;
            }

            var userIds = userAttributes.Select(x => x.UserId);
            var keys = userAttributes.Select(x => x.Key);

            var exists = _userAttributeRepository
                .Get(x => userIds.Contains(x.UserId) && keys.Contains(x.Key))
                .ToList();

            if (exists == null || !exists.Any())
            {
                return Create(userAttributes);
            }

            using (var transaction = _dataConnection.BeginTransaction())
            {
                var attributeResults = new List<UserAttribute>();
                foreach (var item in userAttributes)
                {
                    var exist = exists.FirstOrDefault(x => x.UserId == item.UserId && x.Key.Equals(item.Key));
                    if (exist != null)
                    {
                        exist.Value = item.Value;
                        exist.Expiration = item.Expiration;

                        await _userAttributeRepository.UpdateAsync(exist);
                        attributeResults.Add(exist);
                    }
                    else
                    {
                        var userAttribute = new UserAttribute()
                        {
                            Key = item.Key,
                            UserId = item.UserId,
                            Value = item.Value,
                            Expiration = item.Expiration
                        };

                        _userAttributeRepository.Add(userAttribute);
                        attributeResults.Add(userAttribute);
                    }
                }

                await transaction.CommitAsync();
                return attributeResults.Select(x => x.Id);
            }
        }

        public async Task<int> CreateOrUpdateAsync(long userId, string key, string value, DateTime? expiration = null)
        {
            var exists = _userAttributeRepository.Get(x => x.UserId == userId && x.Key.Equals(key));
            if (exists != null && exists.Any())
            {
                var exist = exists.FirstOrDefault();
                exist.Value = value;
                exist.Expiration = expiration;

                await _userAttributeRepository.UpdateAsync(exist);

                return exist.Id;
            }

            var data = new UserAttribute()
            {
                Key = key,
                UserId = userId,
                Value = value,
                Expiration = expiration
            };

            await _userAttributeRepository.AddAsync(data);
            return data.Id;
        }

        public async Task<bool> DeleteAsync(long userId, string key, string value)
        {
            var exists = _userAttributeRepository.Get(x => x.UserId == userId && x.Key.Equals(key) && x.Value.Equals(value));
            if (exists == null || !exists.Any())
            {
                return false;
            }

            var data = exists.FirstOrDefault();
            await _userAttributeRepository.DeleteAsync(data);

            return true;
        }

        public async Task<bool> DeleteAsync(long userId, string key)
        {
            var exists = _userAttributeRepository.Get(x => x.UserId == userId && x.Key.Equals(key));
            if (exists == null || !exists.Any())
            {
                return false;
            }

            await _userAttributeRepository.DeleteAsync(exists);

            return true;
        }
    }
}
