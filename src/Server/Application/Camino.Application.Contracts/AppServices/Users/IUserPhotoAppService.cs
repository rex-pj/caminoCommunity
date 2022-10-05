using Camino.Shared.Enums;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Application.Contracts.AppServices.Media.Dtos;

namespace Camino.Application.Contracts.AppServices.Users
{
    public interface IUserPhotoAppService
    {
        Task DeleteByUserIdAsync(long userId, UserPictureTypes userPhotoType);
        Task<UserPhotoResult> GetByIdAsync(long id, UserPictureTypes typeId);
        Task<UserPhotoResult> GetByUserIdAsync(long userId, UserPictureTypes typeId);
        Task<IList<UserPhotoResult>> GetListByUserIdsAsync(IEnumerable<long> userIds, UserPictureTypes typeId);
        Task<IEnumerable<UserPhotoResult>> GetUserPhotosAsync(long userId);
        Task<UserPhotoUpdateRequest> UpdateAsync(UserPhotoUpdateRequest request, long userId);
    }
}
