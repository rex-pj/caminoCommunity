using Coco.Business.Contracts;
using Coco.Business.ValidationStrategies;
using Coco.Common.Exceptions;
using Coco.Common.Utils;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Enums;
using Coco.Entities.Dtos;
using Coco.Entities.Dtos.General;
using Coco.IdentityDAL;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Coco.Entities.Domain.Content;
using Coco.DAL;
using System.Collections.Generic;

namespace Coco.Business.Implementation.UserBusiness
{
    public class UserPhotoBusiness : IUserPhotoBusiness
    {
        private readonly IdentityDbContext _identityContext;
        private readonly ContentDbContext _contentDbContext;
        private readonly IRepository<UserPhoto> _userPhotoRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly ValidationStrategyContext _validationStrategyContext;
        public UserPhotoBusiness(IdentityDbContext identityContext, ContentDbContext contentDbContext,
            ValidationStrategyContext validationStrategyContext, IRepository<UserPhoto> userPhotoRepository,
            IRepository<UserInfo> userInfoRepository)
        {
            _identityContext = identityContext;
            _contentDbContext = contentDbContext;
            _userPhotoRepository = userPhotoRepository;
            _userInfoRepository = userInfoRepository;
            _validationStrategyContext = validationStrategyContext;
        }

        public async Task<UserPhotoUpdateDto> UpdateUserPhotoAsync(UserPhotoUpdateDto model, long userId)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

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

            if (model.UserPhotoType == UserPhotoTypeEnum.Avatar)
            {
                _validationStrategyContext.SetStrategy(new AvatarValidationStrategy());
                canUpdate = _validationStrategyContext.Validate(model);
            }
            else if (model.UserPhotoType == UserPhotoTypeEnum.Cover)
            {
                _validationStrategyContext.SetStrategy(new UserCoverValidationStrategy());
                canUpdate = _validationStrategyContext.Validate(model);
            }

            if (!canUpdate && model.UserPhotoType == UserPhotoTypeEnum.Avatar)
            {
                throw new PhotoSizeInvalidException($"{nameof(UserPhotoTypeEnum.Avatar)}Should larger than 100px X 100px");
            }
            else if (!canUpdate)
            {
                throw new PhotoSizeInvalidException($"{nameof(UserPhotoTypeEnum.Cover)}Should larger than 1000px X 300px");
            }

            int maxSize = model.UserPhotoType == UserPhotoTypeEnum.Avatar ? 600 : 1000;
            var newImage = ImageUtils
                .Crop(model.PhotoUrl, model.XAxis, model.YAxis, model.Width, model.Height, model.Scale, maxSize);

            var userPhotoType = (byte)model.UserPhotoType;
            var userPhoto = _userPhotoRepository
                .Get(x => x.UserId == userId && x.TypeId == userPhotoType)
                .FirstOrDefault();

            using (var transaction = _contentDbContext.Database.BeginTransaction())
            {
                model.UserPhotoCode = Guid.NewGuid().ToString();
                if (userPhoto == null)
                {
                    userPhoto = new UserPhoto()
                    {
                        CreatedById = userId,
                        CreatedDate = DateTime.UtcNow,
                        ImageData = newImage,
                        TypeId = (byte)model.UserPhotoType,
                        UserId = userId,
                        Name = model.FileName,
                        Code = model.UserPhotoCode,
                    };

                    _userPhotoRepository.Add(userPhoto);
                }
                else
                {
                    userPhoto.ImageData = newImage;
                    userPhoto.Name = model.FileName;
                    userPhoto.Code = model.UserPhotoCode;
                    _userPhotoRepository.Update(userPhoto);
                }

                await _contentDbContext.SaveChangesAsync();
                transaction.Commit();

                model.PhotoUrl = userPhoto.Code;
                return model;
            }
        }

        public async Task DeleteUserPhotoAsync(long userId, UserPhotoTypeEnum userPhotoType)
        {
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var type = (byte)userPhotoType;
            var userPhoto = _userPhotoRepository
                .Get(x => x.UserId.Equals(userId) && x.TypeId.Equals(type))
                .AsNoTracking()
                .FirstOrDefault();

            if (userPhoto == null)
            {
                return;
            }

            _userPhotoRepository.Delete(userPhoto);
            await _contentDbContext.SaveChangesAsync();
        }

        public async Task<UserPhotoDto> GetUserPhotoByCodeAsync(string code, UserPhotoTypeEnum type)
        {
            var photoType = (byte)type;
            var userPhotos = await _userPhotoRepository.GetAsync(x => x.Code.Equals(code) && x.TypeId.Equals(photoType));

            if (userPhotos == null || !userPhotos.Any())
            {
                return null;
            }

            return userPhotos.Select(x => new UserPhotoDto()
            {
                Code = x.Code,
                Description = x.Description,
                Id = x.Id,
                ImageData = x.ImageData,
                Name = x.Name,
                TypeId = x.TypeId,
                UserId = x.UserId,
                Url = x.Url
            }).FirstOrDefault();
        }

        public async Task<IEnumerable<UserPhotoDto>> GetUserPhotosAsync(long userId)
        {
            var userPhotos = await _userPhotoRepository.GetAsync(x => x.UserId == userId);
            
            return userPhotos.Select(x => new UserPhotoDto()
            {
                Code = x.Code,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                Url = x.Url,
                TypeId = x.TypeId
            });
        }

        public UserPhotoDto GetUserPhotoByUserId(long userId, UserPhotoTypeEnum type)
        {
            var photoType = (byte)type;
            var userPhotos = _userPhotoRepository.Get(x => x.UserId == userId && x.TypeId.Equals(photoType)).AsNoTracking();
            if (userPhotos == null || !userPhotos.Any())
            {
                return null;
            }

            return userPhotos.Select(x => new UserPhotoDto()
            {
                Code = x.Code,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                Url = x.Url,
                TypeId = x.TypeId
            }).FirstOrDefault();
        }
    }
}
