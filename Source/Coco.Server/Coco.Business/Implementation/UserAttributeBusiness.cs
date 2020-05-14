using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.User;
using Coco.IdentityDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation
{
    public class UserAttributeBusiness : IUserAttributeBusiness
    {
        private readonly IRepository<UserAttribute> _userAttributeRepository;
        private readonly IdentityDbContext _identityDbContext;

        public UserAttributeBusiness(IdentityDbContext identityDbContext, IRepository<UserAttribute> userAttributeRepository)
        {
            _userAttributeRepository = userAttributeRepository;
            _identityDbContext = identityDbContext;
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

        public async Task<IEnumerable<UserAttribute>> CreateAsync(IEnumerable<UserAttributeDto> userAttributes)
        {
            if (userAttributes == null || !userAttributes.Any())
            {
                return null;
            }

            var result = new List<UserAttribute>();

            foreach (var item in userAttributes)
            {
                var userAttribute = new UserAttribute()
                {
                    Key = item.Key,
                    UserId = item.UserId,
                    Value = item.Value,
                    Expiration = item.Expiration
                };

                _userAttributeRepository.Add(userAttribute);

                result.Add(userAttribute);
            }

            await _identityDbContext.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<UserAttribute>> CreateOrUpdateAsync(IEnumerable<UserAttributeDto> userAttributes)
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
                return await CreateAsync(userAttributes);
            }

            var result = new List<UserAttribute>();
            foreach (var item in userAttributes)
            {
                var exist = exists.FirstOrDefault(x => x.UserId == item.UserId && x.Key.Equals(item.Key));
                if (exist != null)
                {
                    exist.Value = item.Value;
                    exist.Expiration = item.Expiration;

                    _userAttributeRepository.Update(exist);
                    result.Add(exist);
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
                    result.Add(userAttribute);
                }
            }

            await _identityDbContext.SaveChangesAsync();
            return result;
        }

        public async Task<UserAttribute> CreateOrUpdateAsync(long userId, string key, string value, DateTime? expiration = null)
        {
            var exists = _userAttributeRepository.Get(x => x.UserId == userId && x.Key.Equals(key));
            if (exists != null && exists.Any())
            {
                var exist = exists.FirstOrDefault();
                exist.Value = value;
                exist.Expiration = expiration;

                _userAttributeRepository.Update(exist);
                await _identityDbContext.SaveChangesAsync();

                return exist;
            }

            var data = new UserAttribute()
            {
                Key = key,
                UserId = userId,
                Value = value,
                Expiration = expiration
            };

            _userAttributeRepository.Add(data);
            await _identityDbContext.SaveChangesAsync();
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
            _userAttributeRepository.Delete(data);
            await _identityDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(long userId, string key)
        {
            var exists = _userAttributeRepository.Get(x => x.UserId == userId && x.Key.Equals(key));
            if (exists == null || !exists.Any())
            {
                return false;
            }

            _userAttributeRepository.Delete(exists);
            await _identityDbContext.SaveChangesAsync();

            return true;
        }
    }
}
