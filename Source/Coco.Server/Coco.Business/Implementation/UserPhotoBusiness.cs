using Coco.Business.Contracts;
using Coco.Business.ValidationStrategies;
using Coco.Common.Utils;
using Coco.Contract;
using Coco.Entities.Domain.Dbo;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Enums;
using Coco.Entities.Model;
using Coco.Entities.Model.General;
using Coco.IdentityDAL;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation
{
    public class UserPhotoBusiness : IUserPhotoBusiness
    {
        private readonly IDbContext _identityContext;
        private readonly IRepository<UserPhoto> _userPhotoRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly ValidationStrategyContext _validationStrategyContext;
        public UserPhotoBusiness(IdentityDbContext identityContext,
            ValidationStrategyContext validationStrategyContext,
            IRepository<UserPhoto> userPhotoRepository,
            IRepository<UserInfo> userInfoRepository)
        {
            _identityContext = identityContext;
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

            using (var transaction = _identityContext.Database.BeginTransaction())
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

                await _identityContext.SaveChangesAsync();
                transaction.Commit();

                model.PhotoUrl = userPhoto.Code;
                return model;
            }
        }

        public async Task DeleteAvatarAsync(long userId)
        {
            var avatarType = (byte)UserPhotoTypeEnum.Avatar;
            var userPhoto = _userPhotoRepository
                .Get(x => x.UserId.Equals(userId) && x.TypeId.Equals(avatarType))
                .FirstOrDefault();

            if (userPhoto == null)
            {
                return;
            }

            userPhoto.UserInfo.AvatarUrl = null;
            _userPhotoRepository.Delete(userPhoto);
            await _identityContext.SaveChangesAsync();
        }

        public async Task<UserPhoto> GetAvatarByIdAsync(long id)
        {
            var userPhoto = await _userPhotoRepository.FindAsync(id);
            return userPhoto;
        }

        public UserPhotoModel GetUserPhotoByCodeAsync(string code, UserPhotoTypeEnum type)
        {
            var avatarType = (byte)type;
            var userPhoto = _userPhotoRepository
                .GetAsNoTracking(x => x.Code.Equals(code) && x.TypeId.Equals(avatarType))
                .Select(x => new UserPhotoModel()
                {
                    Code = x.Code,
                    Description = x.Description,
                    Id = x.Id,
                    ImageData = x.ImageData,
                    Name = x.Name,
                    TypeId = x.TypeId,
                    UserId = x.UserId,
                    Url = x.Url
                })
                .FirstOrDefault();

            return userPhoto;
        }
    }
}
