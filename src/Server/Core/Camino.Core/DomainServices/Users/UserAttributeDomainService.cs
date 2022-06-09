using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains;
using Camino.Core.Domains.Users;
using Camino.Core.Domains.Users.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Core.DomainServices.Products.Users
{
    public class UserAttributeDomainService : IUserAttributeDomainService, IScopedDependency
    {
        private readonly IEntityRepository<UserAttribute> _userAttributeEntityRepository;
        private readonly IUserAttributeRepository _userAttributeRepository;
        private readonly IDbContext _dbContext;

        public UserAttributeDomainService(IEntityRepository<UserAttribute> userAttributeEntityRepository,
            IUserAttributeRepository userAttributeRepository,
            IDbContext dbContext)
        {
            _userAttributeEntityRepository = userAttributeEntityRepository;
            _userAttributeRepository = userAttributeRepository;
            _dbContext = dbContext;
        }

        public async Task CreateOrUpdateAsync(IEnumerable<UserAttribute> newUserAttributes)
        {
            var userIds = newUserAttributes.Select(x => x.UserId);
            var keys = newUserAttributes.Select(x => x.Key);
            var exists = _userAttributeEntityRepository
                .Get(x => userIds.Contains(x.UserId) && keys.Contains(x.Key))
                .ToList();

            if (exists == null || !exists.Any())
            {
                await _userAttributeRepository.CreateAsync(newUserAttributes);
                return;
            }

            foreach (var userAttribute in newUserAttributes)
            {
                var existing = exists.FirstOrDefault(x => x.UserId == userAttribute.UserId && x.Key.Equals(userAttribute.Key));
                if (existing != null)
                {
                    existing.Value = userAttribute.Value;
                    existing.Expiration = userAttribute.Expiration;
                    await _userAttributeEntityRepository.UpdateAsync(existing);
                }
                else
                {
                    await _userAttributeEntityRepository.InsertAsync(userAttribute);
                }
            }

            _dbContext.SaveChanges();
        }

        public async Task<int> CreateOrUpdateAsync(long userId, string key, string value, DateTime? expiration = null)
        {
            var existing = (await _userAttributeEntityRepository.GetAsync(x => x.UserId == userId && x.Key.Equals(key)))?.FirstOrDefault();
            if (existing == null)
            {
                var newUserAttribute = new UserAttribute()
                {
                    Key = key,
                    UserId = userId,
                    Value = value,
                    Expiration = expiration
                };
                await _userAttributeEntityRepository.InsertAsync(newUserAttribute);
                await _dbContext.SaveChangesAsync();
                return newUserAttribute.Id;
            }

            existing.Value = value;
            existing.Expiration = expiration;
            await _dbContext.SaveChangesAsync();
            return existing.Id;
        }
    }
}
