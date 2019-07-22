using Coco.Business.Contracts;
using Coco.Business.ValidationStrategies;
using Coco.Common.Utils;
using Coco.Contract;
using Coco.Entities.Domain.Dbo;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Enums;
using Coco.Entities.Model.General;
using Coco.IdentityDAL;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation
{
    public class UserPhotoBusiness : IUserPhotoBusiness
    {
        private readonly IDbContext _dbContext;
        private readonly IRepository<UserPhoto> _userPhotoRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly ValidationStrategyContext _validationStrategyContext;
        public UserPhotoBusiness(IdentityDbContext dbContext,
            ValidationStrategyContext validationStrategyContext,
            IRepository<UserPhoto> userPhotoRepository,
            IRepository<UserInfo> userInfoRepository)
        {
            _dbContext = dbContext;
            _userPhotoRepository = userPhotoRepository;
            _userInfoRepository = userInfoRepository;
            _validationStrategyContext = validationStrategyContext;
        }

        public async Task<UpdateAvatarModel> UpdateAvatarAsync(UpdateAvatarModel model, long userId)
        {
            var userInfo = _userInfoRepository.Find(userId);
            if (userInfo == null)
            {
                throw new ArgumentException(nameof(userInfo));
            }

            _validationStrategyContext.SetStrategy(new Base64ImageValidationStrategy());
            bool canUpdate = _validationStrategyContext.Validate(model.PhotoUrl);

            if (!canUpdate)
            {
                throw new ArgumentException(model.PhotoUrl);
            }

            _validationStrategyContext.SetStrategy(new AvatarValidationStrategy());
            canUpdate = _validationStrategyContext.Validate(model);

            if (!canUpdate)
            {
                throw new ArgumentException("Avatar Url");
            }

            var newImage = ImageUtils
                .Crop(model.PhotoUrl, model.XAxis, model.YAxis, model.Width, model.Height);

            var avatarType = (byte)UserPhotoTypeEnum.Avatar;
            var userPhoto = _userPhotoRepository
                .Get(x => x.UserId == userId && x.TypeId == avatarType)
                .FirstOrDefault();

            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                if (userPhoto == null)
                {
                    userPhoto = new UserPhoto()
                    {
                        CreatedById = userId,
                        CreatedDate = DateTime.Now,
                        ImageData = newImage,
                        TypeId = (byte)UserPhotoTypeEnum.Avatar,
                        UserId = userId,
                        Name = model.FileName,
                        Code = model.AvatarCode,
                    };

                    _userPhotoRepository.Insert(userPhoto);
                }
                else
                {
                    userPhoto.ImageData = newImage;
                    userPhoto.Name = model.FileName;
                    userPhoto.Code = model.AvatarCode;
                    _userPhotoRepository.Update(userPhoto);
                }

                userInfo.AvatarUrl = model.AvatarCode;

                await _dbContext.SaveChangesAsync();
                transaction.Commit();

                model.PhotoUrl = userPhoto.Code;
                return model;
            }
            
        }

        public async Task<UserPhoto> GetAvatarByIdAsync(long id)
        {
            var userPhoto = await _userPhotoRepository.FindAsync(id);
            return userPhoto;
        }

        public async Task<UserPhoto> GetAvatarByCodeAsync(string code)
        {
            var avatarType = (byte)UserPhotoTypeEnum.Avatar;
            var userPhotos = await _userPhotoRepository
                .GetAsNoTrackingAsync(x => x.Code == code && x.TypeId == avatarType);

            if (userPhotos != null && userPhotos.Any())
            {
                return userPhotos.FirstOrDefault();
            }

            return null;
        }
    }
}
