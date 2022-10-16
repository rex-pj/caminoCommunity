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
        private readonly BaseValidatorContext _validatorContext;
        public UserPhotoAppService(IUserPhotoRepository userPhotoRepository,
            IUserRepository userRepository,
            BaseValidatorContext validatorContext)
        {
            _userPhotoRepository = userPhotoRepository;
            _userRepository = userRepository;
            _validatorContext = validatorContext;
        }

        public async Task<long> UpdateAsync(UserPhotoUpdateRequest request, long userId)
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

            _validatorContext.SetValidator(new ImageFileValidator());
            bool canUpdate = _validatorContext.Validate<byte[], bool>(request.FileData);
            if (!canUpdate)
            {
                throw new ArgumentException(nameof(request.FileData));
            }

            switch (request.UserPhotoTypeId)
            {
                case (int)UserPictureTypes.Avatar:
                    _validatorContext.SetValidator(new AvatarValidator());
                    canUpdate = _validatorContext.Validate<UserPhotoUpdateRequest, bool>(request);
                    break;
                case (int)UserPictureTypes.Cover:
                    _validatorContext.SetValidator(new UserCoverValidator());
                    canUpdate = _validatorContext.Validate<UserPhotoUpdateRequest, bool>(request);
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
                .Crop(request.FileData, request.XAxis, request.YAxis, request.Width, request.Height, request.Scale, maxSize);

            var userPhotoType = (UserPictureTypes)request.UserPhotoTypeId;
            var userPhoto = await _userPhotoRepository
                .GetByUserIdAsync(userId, userPhotoType);

            if (userPhoto == null)
            {
                userPhoto = new UserPhoto()
                {
                    CreatedById = userId,
                    CreatedDate = DateTime.UtcNow,
                    FileData = newImage,
                    TypeId = request.UserPhotoTypeId,
                    UserId = userId,
                    Name = request.FileName,
                };
                await _userPhotoRepository.CreateAsync(userPhoto);
            }
            else
            {
                userPhoto.FileData = newImage;
                userPhoto.Name = request.FileName;
                await _userPhotoRepository.UpdateAsync(userPhoto);
            }

            return userPhoto.Id;
        }

        public async Task DeleteByUserIdAsync(long userId, UserPictureTypes userPhotoType)
        {
            await _userPhotoRepository.DeleteByUserIdAsync(userId, userPhotoType);
        }

        public async Task<UserPhotoResult> GetByIdAsync(long id, UserPictureTypes typeId)
        {
            var existing = await _userPhotoRepository.GetByIdAsync(id, typeId);
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
                Description = entity.Description,
                Id = entity.Id,
                FileData = entity.FileData,
                Name = entity.Name,
                TypeId = entity.TypeId,
                UserId = entity.UserId
            };
        }
    }
}
