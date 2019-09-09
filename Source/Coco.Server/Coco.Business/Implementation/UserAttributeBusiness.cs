using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.IdentityDAL;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation
{
    public class UserAttributeBusiness : IUserAttributeBusiness
    {
        private readonly IRepository<UserAttribute> _userAttributeRepository;
        private readonly IdentityDbContext _dbContext;

        public UserAttributeBusiness(IdentityDbContext dbContext, IRepository<UserAttribute> userAttributeRepository)
        {
            _userAttributeRepository = userAttributeRepository;
            _dbContext = dbContext;
        }

        public async Task<UserAttribute> GetAsync(long userId, string key)
        {
            var exists = await _userAttributeRepository.GetAsync(x => x.UserId == userId && x.Key.Equals(key));
            if(exists!=null && exists.Any())
            {
                var data = exists.FirstOrDefault();

                return data;
            }
            return null;
        }

        public async Task<int> CreateOrUpdateAsync(long userId, string key, string value)
        {
            var exists = await _userAttributeRepository.GetAsync(x => x.UserId == userId && x.Key.Equals(key));
            if (exists != null && exists.Any())
            {
                var exist = exists.FirstOrDefault();
                exist.Value = value;

                _userAttributeRepository.Update(exist);
                await _dbContext.SaveChangesAsync();

                return exist.Id;
            }

            var data = new UserAttribute()
            {
                Key = key,
                UserId = userId,
                Value = value
            };

            _userAttributeRepository.Add(data);
            return data.Id;
        }
    }
}
