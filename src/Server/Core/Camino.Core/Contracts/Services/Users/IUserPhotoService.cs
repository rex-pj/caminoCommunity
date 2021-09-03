using Camino.Shared.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Shared.Results.Media;
using Camino.Shared.Requests.Identifiers;

namespace Camino.Core.Contracts.Services.Users
{
    public interface IUserPhotoService
    {
        Task<UserPhotoUpdateRequest> UpdateUserPhotoAsync(UserPhotoUpdateRequest request, long userId);
        Task DeleteUserPhotoAsync(long userId, UserPictureType userPhotoType);
        Task<UserPhotoResult> GetUserPhotoByCodeAsync(string code, UserPictureType typeId);
        UserPhotoResult GetUserPhotoByUserId(long userId, UserPictureType typeId);
        Task<IEnumerable<UserPhotoResult>> GetUserPhotosAsync(long userId);
        Task<IList<UserPhotoResult>> GetUserPhotoByUserIdsAsync(IEnumerable<long> userIds, UserPictureType typeId);
    }
}
