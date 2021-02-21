using Camino.Core.Exceptions;
using Camino.Core.Utils;
using Camino.Core.Contracts.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Shared.Enums;
using Camino.Shared.Results.Media;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Domain.Media;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Requests.Identifiers;
using Camino.Infrastructure.Strategies.Validations;
using LinqToDB;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.Service.Repository.Users
{
    public class UserPhotoRepository : IUserPhotoRepository
    {
        private readonly IRepository<UserPhoto> _userPhotoRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly ValidationStrategyContext _validationStrategyContext;
        public UserPhotoRepository(ValidationStrategyContext validationStrategyContext, IServiceScopeFactory serviceScopeFactory,
            IRepository<UserInfo> userInfoRepository)
        {
            _userPhotoRepository = serviceScopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<IRepository<UserPhoto>>();
            _userInfoRepository = userInfoRepository;
            _validationStrategyContext = validationStrategyContext;
        }

        public async Task<UserPhotoUpdateRequest> UpdateUserPhotoAsync(UserPhotoUpdateRequest request, long userId)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var userInfo = _userInfoRepository.FirstOrDefault(x => x.Id == userId);
            if (userInfo == null)
            {
                throw new ArgumentException(nameof(userInfo));
            }

            _validationStrategyContext.SetStrategy(new Base64ImageValidationStrategy());
            bool canUpdate = _validationStrategyContext.Validate(request.PhotoUrl);

            if (!canUpdate)
            {
                throw new ArgumentException(request.PhotoUrl);
            }

            if (request.UserPhotoType == UserPhotoKind.Avatar)
            {
                _validationStrategyContext.SetStrategy(new AvatarValidationStrategy());
                canUpdate = _validationStrategyContext.Validate(request);
            }
            else if (request.UserPhotoType == UserPhotoKind.Cover)
            {
                _validationStrategyContext.SetStrategy(new UserCoverValidationStrategy());
                canUpdate = _validationStrategyContext.Validate(request);
            }

            if (!canUpdate && request.UserPhotoType == UserPhotoKind.Avatar)
            {
                throw new PhotoSizeInvalidException($"{nameof(UserPhotoKind.Avatar)}Should larger than 100px X 100px");
            }
            else if (!canUpdate)
            {
                throw new PhotoSizeInvalidException($"{nameof(UserPhotoKind.Cover)}Should larger than 1000px X 300px");
            }

            int maxSize = request.UserPhotoType == UserPhotoKind.Avatar ? 600 : 1000;
            var newImage = ImageUtil
                .Crop(request.PhotoUrl, request.XAxis, request.YAxis, request.Width, request.Height, request.Scale, maxSize);

            var userPhotoType = (byte)request.UserPhotoType;
            var userPhoto = _userPhotoRepository
                .Get(x => x.UserId == userId && x.TypeId == userPhotoType)
                .FirstOrDefault();

            request.UserPhotoCode = Guid.NewGuid().ToString();
            if (userPhoto == null)
            {
                userPhoto = new UserPhoto()
                {
                    CreatedById = userId,
                    CreatedDate = DateTime.UtcNow,
                    ImageData = newImage,
                    TypeId = (byte)request.UserPhotoType,
                    UserId = userId,
                    Name = request.FileName,
                    Code = request.UserPhotoCode,
                };

                await _userPhotoRepository.AddAsync(userPhoto);
            }
            else
            {
                userPhoto.ImageData = newImage;
                userPhoto.Name = request.FileName;
                userPhoto.Code = request.UserPhotoCode;
                await _userPhotoRepository.UpdateAsync(userPhoto);
            }
            request.PhotoUrl = userPhoto.Code;
            return request;
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

        public UserPhotoResult GetUserPhotoByUserId(long userId, UserPhotoKind typeId)
        {
            var photoType = (byte)typeId;
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

        public IList<UserPhotoResult> GetUserPhotoByUserIds(IEnumerable<long> userIds, UserPhotoKind typeId)
        {
            var photoType = (byte)typeId;
            var userPhotos = _userPhotoRepository.Get(x => userIds.Contains(x.UserId) && x.TypeId.Equals(photoType));

            return userPhotos.Select(x => new UserPhotoResult()
            {
                Code = x.Code,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                Url = x.Url,
                TypeId = x.TypeId,
                UserId = x.UserId
            }).ToList();
        }

        public async Task<string> GetCodeByUserId(long userId, UserPhotoKind typeId)
        {
            var photoType = (byte)typeId;
            var userPhotoCode = await _userPhotoRepository.Get(x => x.UserId == userId && x.TypeId.Equals(photoType))
                .Select(x => x.Code).FirstOrDefaultAsync();

            return userPhotoCode;
        }

        public async Task<IList<UserPhotoResult>> GetUserPhotosByUserIds(IEnumerable<long> userIds, UserPhotoKind typeId)
        {
            var photoType = (byte)typeId;
            var userPhotoCodes = await _userPhotoRepository.Get(x => userIds.Contains(x.UserId) && x.TypeId.Equals(photoType))
                .Select(x => new UserPhotoResult
                {
                    Code = x.Code,
                    UserId = x.UserId
                }).ToListAsync();

            return userPhotoCodes;
        }
    }
}
