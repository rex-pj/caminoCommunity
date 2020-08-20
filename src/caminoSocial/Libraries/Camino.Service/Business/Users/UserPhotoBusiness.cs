using Camino.Service.Strategies.Validation;
using Camino.Core.Exceptions;
using Camino.Core.Utils;
using Camino.Data.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Service.Data.Content;
using Camino.Data.Enums;
using Camino.Service.Business.Users.Contracts;
using Camino.DAL.Entities;
using Camino.IdentityDAL.Entities;
using Camino.Service.Data.Request;

namespace Camino.Service.Business.Users
{
    public class UserPhotoBusiness : IUserPhotoBusiness
    {
        private readonly IRepository<UserPhoto> _userPhotoRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly ValidationStrategyContext _validationStrategyContext;
        public UserPhotoBusiness(ValidationStrategyContext validationStrategyContext, IRepository<UserPhoto> userPhotoRepository,
            IRepository<UserInfo> userInfoRepository)
        {
            _userPhotoRepository = userPhotoRepository;
            _userInfoRepository = userInfoRepository;
            _validationStrategyContext = validationStrategyContext;
        }

        public async Task<UserPhotoUpdateRequest> UpdateUserPhotoAsync(UserPhotoUpdateRequest model, long userId)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userInfo = _userInfoRepository.FirstOrDefault(x => x.Id == userId);
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

            if (model.UserPhotoType == UserPhotoKind.Avatar)
            {
                _validationStrategyContext.SetStrategy(new AvatarValidationStrategy());
                canUpdate = _validationStrategyContext.Validate(model);
            }
            else if (model.UserPhotoType == UserPhotoKind.Cover)
            {
                _validationStrategyContext.SetStrategy(new UserCoverValidationStrategy());
                canUpdate = _validationStrategyContext.Validate(model);
            }

            if (!canUpdate && model.UserPhotoType == UserPhotoKind.Avatar)
            {
                throw new PhotoSizeInvalidException($"{nameof(UserPhotoKind.Avatar)}Should larger than 100px X 100px");
            }
            else if (!canUpdate)
            {
                throw new PhotoSizeInvalidException($"{nameof(UserPhotoKind.Cover)}Should larger than 1000px X 300px");
            }

            int maxSize = model.UserPhotoType == UserPhotoKind.Avatar ? 600 : 1000;
            var newImage = ImageUtil
                .Crop(model.PhotoUrl, model.XAxis, model.YAxis, model.Width, model.Height, model.Scale, maxSize);

            var userPhotoType = (byte)model.UserPhotoType;
            var userPhoto = _userPhotoRepository
                .Get(x => x.UserId == userId && x.TypeId == userPhotoType)
                .FirstOrDefault();

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

                await _userPhotoRepository.AddAsync(userPhoto);
            }
            else
            {
                userPhoto.ImageData = newImage;
                userPhoto.Name = model.FileName;
                userPhoto.Code = model.UserPhotoCode;
                await _userPhotoRepository.UpdateAsync(userPhoto);
            }
            model.PhotoUrl = userPhoto.Code;
            return model;
        }

        public async Task DeleteUserPhotoAsync(long userId, UserPhotoKind userPhotoType)
        {
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var type = (byte)userPhotoType;
            var userPhoto = _userPhotoRepository
                .Get(x => x.UserId.Equals(userId) && x.TypeId.Equals(type))
                .FirstOrDefault();

            if (userPhoto == null)
            {
                return;
            }

            await _userPhotoRepository.DeleteAsync(userPhoto);
        }

        public async Task<UserPhotoResult> GetUserPhotoByCodeAsync(string code, UserPhotoKind type)
        {
            var photoType = (byte)type;
            var userPhotos = await _userPhotoRepository.GetAsync(x => x.Code.Equals(code) && x.TypeId.Equals(photoType));

            if (userPhotos == null || !userPhotos.Any())
            {
                return null;
            }

            return userPhotos.Select(x => new UserPhotoResult()
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

        public async Task<IEnumerable<UserPhotoResult>> GetUserPhotosAsync(long userId)
        {
            var userPhotos = await _userPhotoRepository.GetAsync(x => x.UserId == userId);
            
            return userPhotos.Select(x => new UserPhotoResult()
            {
                Code = x.Code,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                Url = x.Url,
                TypeId = x.TypeId
            });
        }

        public UserPhotoResult GetUserPhotoByUserId(long userId, UserPhotoKind type)
        {
            var photoType = (byte)type;
            var userPhotos = _userPhotoRepository.Get(x => x.UserId == userId && x.TypeId.Equals(photoType));
            if (userPhotos == null || !userPhotos.Any())
            {
                return null;
            }

            return userPhotos.Select(x => new UserPhotoResult()
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
