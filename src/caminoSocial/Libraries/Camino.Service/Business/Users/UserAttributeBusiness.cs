using Camino.Data.Contracts;
using Camino.Service.Data.Identity;
using Camino.IdentityDAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Users.Contracts;
using Camino.IdentityDAL.Entities;

namespace Camino.Service.Business.Users
{
    public class UserAttributeBusiness : IUserAttributeBusiness
    {
        private readonly IRepository<UserAttribute> _userAttributeRepository;
        private readonly IIdentityDataProvider _identityDataProvider;

        public UserAttributeBusiness(IRepository<UserAttribute> userAttributeRepository, IIdentityDataProvider identityDataProvider)
        {
            _userAttributeRepository = userAttributeRepository;
            _identityDataProvider = identityDataProvider;
        }

        public async Task<UserAttribute> GetAsync(long userId, string key)
        {
            var exists = await _userAttributeRepository.GetAsync(x => x.UserId == userId && x.Key.Equals(key));
            if (exists == null || !exists.Any())
            {
                return null;
            }

            var data = exists.FirstOrDefault();
            return data;
        }

        public async Task<IEnumerable<UserAttribute>> GetAsync(long userId)
        {
            var exists = await _userAttributeRepository.GetAsync(x => x.UserId == userId);
            return exists.ToList();
        }

        public IEnumerable<UserAttribute> Get(long userId)
        {
            var exists = _userAttributeRepository.Get(x => x.UserId == userId);
            return exists.ToList();
        }

        public IEnumerable<UserAttribute> Create(IEnumerable<UserAttributeResult> attributes)
        {
            if (attributes == null || !attributes.Any())
            {
                return null;
            }

            var userAttributes = new List<UserAttribute>();

            foreach (var item in attributes)
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
            return userAttributes;
        }

        public async Task<IEnumerable<UserAttribute>> CreateOrUpdateAsync(IEnumerable<UserAttributeResult> userAttributes)
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

            using (var transaction = _identityDataProvider.BeginTransaction())
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

                transaction.Commit();
                return attributeResults;
            }
        }

        public async Task<UserAttribute> CreateOrUpdateAsync(long userId, string key, string value, DateTime? expiration = null)
        {
            var exists = _userAttributeRepository.Get(x => x.UserId == userId && x.Key.Equals(key));
            if (exists != null && exists.Any())
            {
                var exist = exists.FirstOrDefault();
                exist.Value = value;
                exist.Expiration = expiration;

                await _userAttributeRepository.UpdateAsync(exist);

                return exist;
            }

            var data = new UserAttribute()
            {
                Key = key,
                UserId = userId,
                Value = value,
                Expiration = expiration
            };

            await _userAttributeRepository.AddAsync(data);
            return data;
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
