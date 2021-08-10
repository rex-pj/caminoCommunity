using Camino.Shared.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Shared.Results.Media;
using Camino.Shared.Requests.Identifiers;

namespace Camino.Core.Contracts.Repositories.Users
{
    public interface IUserPhotoRepository
    {
        Task<UserPhotoUpdateRequest> UpdateUserPhotoAsync(UserPhotoUpdateRequest request, long userId);
        Task DeleteUserPhotoAsync(long userId, UserPictureType userPhotoType);
        Task<UserPhotoResult> GetUserPhotoByCodeAsync(string code, UserPictureType type);
        UserPhotoResult GetUserPhotoByUserId(long userId, UserPictureType type);
        Task<IEnumerable<UserPhotoResult>> GetUserPhotosAsync(long userId);
        Task<IList<UserPhotoResult>> GetUserPhotoByUserIdsAsync(IEnumerable<long> userIds, UserPictureType typeId);
        Task<IList<UserPhotoResult>> GetUserPhotosByUserIdsAsync(IEnumerable<long> userIds, UserPictureType typeId);
        Task<string> GetCodeByUserIdAsync(long userId, UserPictureType typeId);
    }
}
