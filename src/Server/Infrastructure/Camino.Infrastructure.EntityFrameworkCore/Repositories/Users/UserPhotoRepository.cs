using Camino.Shared.Enums;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Domains.Media;
using Microsoft.EntityFrameworkCore;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Users
{
    public class UserPhotoRepository : IUserPhotoRepository, IScopedDependency
    {
        private readonly IEntityRepository<UserPhoto> _userPhotoEntityRepository;
        private readonly IAppDbContext _dbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public UserPhotoRepository(IEntityRepository<UserPhoto> userPhotoEntityRepository, IAppDbContext dbContext, IServiceScopeFactory serviceScopeFactory)
        {
            _userPhotoEntityRepository = userPhotoEntityRepository;
            _serviceScopeFactory = serviceScopeFactory;
            _dbContext = dbContext;
        }

        public async Task<long> CreateAsync(UserPhoto userPhoto)
        {
            await _userPhotoEntityRepository.InsertAsync(userPhoto);
            await _dbContext.SaveChangesAsync();
            return userPhoto.Id;
        }

        public async Task UpdateAsync(UserPhoto userPhoto)
        {
            await _userPhotoEntityRepository.UpdateAsync(userPhoto);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteByUserIdAsync(long userId, UserPictureTypes userPhotoType)
        {
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var type = (int)userPhotoType;
            var userPhoto = _userPhotoEntityRepository
                .Get(x => x.UserId.Equals(userId) && x.TypeId.Equals(type))
                .FirstOrDefault();

            if (userPhoto == null)
            {
                return;
            }

            await _userPhotoEntityRepository.DeleteAsync(userPhoto);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserPhoto> GetByCodeAsync(string code, UserPictureTypes type)
        {
            var photoType = (int)type;
            var userPhotos = await _userPhotoEntityRepository.GetAsync(x => x.Code.Equals(code) && x.TypeId.Equals(photoType));
            if (userPhotos == null || !userPhotos.Any())
            {
                return null;
            }

            return userPhotos.FirstOrDefault();
        }
        
        public async Task<UserPhoto> GetByUserIdAsync(long userId, UserPictureTypes typeId)
        {
            var photoType = (int)typeId;
            var userPhoto = (await _userPhotoEntityRepository
                .GetAsync(x => x.UserId == userId && x.TypeId.Equals(photoType)))?.FirstOrDefault();

            return userPhoto;
        }

        public async Task<IEnumerable<UserPhoto>> GetListAsync(long userId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userPhotoRepository = scope.ServiceProvider.GetRequiredService<IEntityRepository<UserPhoto>>();
                var userPhotos = await userPhotoRepository.GetAsync(x => x.UserId == userId);
                if (userPhotos == null || !userPhotos.Any())
                {
                    return new List<UserPhoto>();
                }

                return userPhotos;
            }               
        }

        public async Task<string> GetCodeByUserIdAsync(long userId, UserPictureTypes typeId)
        {
            var photoType = (int)typeId;
            var userPhotoCode = await _userPhotoEntityRepository.Get(x => x.UserId == userId && x.TypeId.Equals(photoType))
                .Select(x => x.Code).FirstOrDefaultAsync();

            return userPhotoCode;
        }

        public async Task<IList<UserPhoto>> GetListByUserIdsAsync(IEnumerable<long> userIds, UserPictureTypes typeId)
        {
            var photoType = (int)typeId;
            var userPhotos = await _userPhotoEntityRepository.GetAsync(x => userIds.Contains(x.UserId) && x.TypeId.Equals(photoType));
            if (userPhotos == null || !userPhotos.Any())
            {
                return new List<UserPhoto>();
            }

            return userPhotos;
        }
    }
}
