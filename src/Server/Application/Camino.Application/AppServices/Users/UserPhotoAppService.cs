using Camino.Shared.Enums;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Application.Contracts.AppServices.Users;
using Camino.Core.DependencyInjection;
using Camino.Core.Validators;
using Camino.Application.Validators;
using Camino.Core.Domains.Media;
using Camino.Application.Contracts.AppServices.Media.Dtos;
using Camino.Shared.Exceptions;
using Camino.Shared.Utils;

namespace Camino.Application.AppServices.Users
{
    public class UserPhotoAppService : IUserPhotoAppService, IScopedDependency
    {
        private readonly IUserPhotoRepository _userPhotoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidationStrategyContext _validationStrategyContext;
        public UserPhotoAppService(IUserPhotoRepository userPhotoRepository,
            IUserRepository userRepository,
            IValidationStrategyContext validationStrategyContext)
        {
            _userPhotoRepository = userPhotoRepository;
            _userRepository = userRepository;
            _validationStrategyContext = validationStrategyContext;
        }

        public async Task<UserPhotoUpdateRequest> UpdateAsync(UserPhotoUpdateRequest request, long userId)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var userInfo = await _userRepository.FindByIdAsync(userId);
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

            switch (request.UserPhotoTypeId)
            {
                case (int)UserPictureTypes.Avatar:
                    _validationStrategyContext.SetStrategy(new AvatarValidationStrategy());
                    canUpdate = _validationStrategyContext.Validate(request);
                    break;
                case (int)UserPictureTypes.Cover:
                    _validationStrategyContext.SetStrategy(new UserCoverValidationStrategy());
                    canUpdate = _validationStrategyContext.Validate(request);
                    break;
            }

            if (!canUpdate && request.UserPhotoTypeId == (int)UserPictureTypes.Avatar)
            {
                throw new PhotoSizeInvalidException($"{nameof(UserPictureTypes.Avatar)} Should larger than 100px X 100px");
            }
            else if (!canUpdate)
            {
                throw new PhotoSizeInvalidException($"{nameof(UserPictureTypes.Cover)} Should larger than 1000px X 300px");
            }

            int maxSize = request.UserPhotoTypeId == (int)UserPictureTypes.Avatar ? 600 : 1000;
            var newImage = ImageUtils
                .Crop(request.PhotoUrl, request.XAxis, request.YAxis, request.Width, request.Height, request.Scale, maxSize);

            var userPhotoType = (UserPictureTypes)request.UserPhotoTypeId;
            var userPhoto = await _userPhotoRepository
                .GetByUserIdAsync(userId, userPhotoType);

            if (userPhoto == null)
            {
                userPhoto = new UserPhoto()
                {
                    CreatedById = userId,
                    CreatedDate = DateTime.UtcNow,
                    ImageData = newImage,
                    TypeId = request.UserPhotoTypeId,
                    UserId = userId,
                    Name = request.FileName,
                    Code = request.UserPhotoCode,
                };
                await _userPhotoRepository.CreateAsync(userPhoto);
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

        public async Task DeleteByUserIdAsync(long userId, UserPictureTypes userPhotoType)
        {
            await _userPhotoRepository.DeleteByUserIdAsync(userId, userPhotoType);
        }

        public async Task<UserPhotoResult> GetByCodeAsync(string code, UserPictureTypes typeId)
        {
            var existing = await _userPhotoRepository.GetByCodeAsync(code, typeId);
            return MapEntityToDto(existing);
        }

        public async Task<IEnumerable<UserPhotoResult>> GetUserPhotosAsync(long userId)
        {
            var userPhotos = await _userPhotoRepository.GetListAsync(userId);
            if (userPhotos == null || !userPhotos.Any())
            {
                return new List<UserPhotoResult>();
            }

            return userPhotos.Select(x => MapEntityToDto(x)).ToList();
        }

        public async Task<UserPhotoResult> GetByUserIdAsync(long userId, UserPictureTypes typeId)
        {
            var existing = await _userPhotoRepository.GetByUserIdAsync(userId, typeId);
            return MapEntityToDto(existing);
        }

        public async Task<string> GetCodeByUserIdAsync(long userId, UserPictureTypes typeId)
        {
            var userPhoto = await GetByUserIdAsync(userId, typeId);
            return userPhoto.Code;
        }

        public async Task<IList<UserPhotoResult>> GetListByUserIdsAsync(IEnumerable<long> userIds, UserPictureTypes typeId)
        {
            var userPhotos = await _userPhotoRepository.GetListByUserIdsAsync(userIds, typeId);
            if (userPhotos == null || !userPhotos.Any())
            {
                return new List<UserPhotoResult>();
            }

            return userPhotos.Select(x => MapEntityToDto(x)).ToList();
        }

        private UserPhotoResult MapEntityToDto(UserPhoto entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new UserPhotoResult
            {
                Code = entity.Code,
                Description = entity.Description,
                Id = entity.Id,
                ImageData = entity.ImageData,
                Name = entity.Name,
                TypeId = entity.TypeId,
                UserId = entity.UserId,
                Url = entity.Url,
                BinaryData = Convert.FromBase64String(entity.ImageData)
            };
        }
    }
}
