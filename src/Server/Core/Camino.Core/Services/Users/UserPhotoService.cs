using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Shared.Enums;
using Camino.Shared.Results.Media;
using Camino.Core.Contracts.Services.Users;
using Camino.Shared.Requests.Identifiers;
using Camino.Core.Contracts.Repositories.Users;
using System;

namespace Camino.Services.Users
{
    public class UserPhotoService : IUserPhotoService
    {
        private readonly IUserPhotoRepository _userPhotoRepository;
        public UserPhotoService(IUserPhotoRepository userPhotoRepository)
        {
            _userPhotoRepository = userPhotoRepository;
        }

        public async Task<UserPhotoUpdateRequest> UpdateUserPhotoAsync(UserPhotoUpdateRequest request, long userId)
        {
            return await _userPhotoRepository.UpdateUserPhotoAsync(request, userId);
        }

        public async Task DeleteUserPhotoAsync(long userId, UserPhotoKind userPhotoType)
        {
            await _userPhotoRepository.DeleteUserPhotoAsync(userId, userPhotoType);
        }

        public async Task<UserPhotoResult> GetUserPhotoByCodeAsync(string code, UserPhotoKind typeId)
        {
            var picture = await _userPhotoRepository.GetUserPhotoByCodeAsync(code, typeId);
            picture.BinaryData = Convert.FromBase64String(picture.ImageData);
            return picture;
        }

        public async Task<IEnumerable<UserPhotoResult>> GetUserPhotosAsync(long userId)
        {
            return await _userPhotoRepository.GetUserPhotosAsync(userId);
        }

        public UserPhotoResult GetUserPhotoByUserId(long userId, UserPhotoKind typeId)
        {
            return _userPhotoRepository.GetUserPhotoByUserId(userId, typeId);
        }

        public IList<UserPhotoResult> GetUserPhotoByUserIds(IEnumerable<long> userIds, UserPhotoKind typeId)
        {
            return _userPhotoRepository.GetUserPhotoByUserIds(userIds, typeId);
        }
    }
}
