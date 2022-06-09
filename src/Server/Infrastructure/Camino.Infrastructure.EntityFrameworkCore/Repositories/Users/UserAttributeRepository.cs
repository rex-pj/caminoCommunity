using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Camino.Core.Domains.Users;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;
using System;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Users
{
    public class UserAttributeRepository : IUserAttributeRepository, IScopedDependency
    {
        private readonly IEntityRepository<UserAttribute> _userAttributeRepository;
        private readonly IAppDbContext _dbContext;

        public UserAttributeRepository(IEntityRepository<UserAttribute> userAttributeRepository, IAppDbContext dbContext)
        {
            _userAttributeRepository = userAttributeRepository;
            _dbContext = dbContext;
        }

        public async Task<UserAttribute> GetAsync(long userId, string key)
        {
            var existing = await _userAttributeRepository
                .Get(x => x.UserId == userId && x.Key.Equals(key))?.FirstOrDefaultAsync();
            return existing;
        }

        public async Task<IEnumerable<UserAttribute>> GetAsync(long userId)
        {
            var exists = _userAttributeRepository.Get(x => x.UserId == userId);
            return await exists.ToListAsync();
        }

        public async Task CreateAsync(IEnumerable<UserAttribute> userAttributes)
        {
            if (userAttributes == null || !userAttributes.Any())
            {
                throw new ArgumentNullException(nameof(userAttributes));
            }

            await _userAttributeRepository.InsertAsync(userAttributes);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CreateAsync(UserAttribute userAttribute, bool needSaveChanges = false)
        {
            await _userAttributeRepository.InsertAsync(userAttribute);
            if (needSaveChanges)
            {
                await _dbContext.SaveChangesAsync();
            }

            return userAttribute.Id;
        }

        public async Task<int> UpdateAsync(UserAttribute userAttribute, bool needSaveChanges = false)
        {
            await _userAttributeRepository.UpdateAsync(userAttribute);
            if (needSaveChanges)
            {
                await _dbContext.SaveChangesAsync();
            }

            return userAttribute.Id;
        }

        public async Task<bool> DeleteAsync(long userId, string key, string value)
        {
            var existing = (await _userAttributeRepository.GetAsync(x => x.UserId == userId && x.Key.Equals(key) && x.Value.Equals(value)))
                ?.FirstOrDefault();
            return await DeleteAsync(existing);
        }

        public async Task<bool> DeleteAsync(long userId, string key)
        {
            var existing = (await _userAttributeRepository
                .GetAsync(x => x.UserId == userId && x.Key.Equals(key)))
                ?.FirstOrDefault();

            return await DeleteAsync(existing);
        }

        private async Task<bool> DeleteAsync(UserAttribute userAttribute)
        {
            if (userAttribute == null)
            {
                throw new ArgumentNullException(nameof(userAttribute));
            }

            await _userAttributeRepository.DeleteAsync(userAttribute);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
