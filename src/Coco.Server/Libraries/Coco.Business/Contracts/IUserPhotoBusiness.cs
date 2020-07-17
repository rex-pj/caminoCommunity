using Coco.Core.Entities.Enums;
using Coco.Core.Dtos.General;
using System.Threading.Tasks;
using System.Collections.Generic;
using Coco.Core.Dtos.Content;

namespace Coco.Business.Contracts
{
    public interface IUserPhotoBusiness
    {
        Task<UserPhotoUpdateDto> UpdateUserPhotoAsync(UserPhotoUpdateDto model, long userId);
        Task DeleteUserPhotoAsync(long userId, UserPhotoType userPhotoType);
        Task<UserPhotoDto> GetUserPhotoByCodeAsync(string code, UserPhotoType type);
        UserPhotoDto GetUserPhotoByUserId(long userId, UserPhotoType type);
        Task<IEnumerable<UserPhotoDto>> GetUserPhotosAsync(long userId);
    }
}
