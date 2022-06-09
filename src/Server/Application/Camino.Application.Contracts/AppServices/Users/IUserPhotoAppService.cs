using Camino.Shared.Enums;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Application.Contracts.AppServices.Media.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Camino.Application.Contracts.AppServices.Users
{
    public interface IUserPhotoAppService
    {
        Task DeleteByUserIdAsync(long userId, UserPictureTypes userPhotoType);
        Task<UserPhotoResult> GetByCodeAsync(string code, UserPictureTypes typeId);
        Task<UserPhotoResult> GetByUserIdAsync(long userId, UserPictureTypes typeId);
        Task<IList<UserPhotoResult>> GetListByUserIdsAsync(IEnumerable<long> userIds, UserPictureTypes typeId);
        Task<string> GetCodeByUserIdAsync(long userId, UserPictureTypes typeId);
        Task<IEnumerable<UserPhotoResult>> GetUserPhotosAsync(long userId);
        Task<UserPhotoUpdateRequest> UpdateAsync(UserPhotoUpdateRequest request, long userId);
    }
}
