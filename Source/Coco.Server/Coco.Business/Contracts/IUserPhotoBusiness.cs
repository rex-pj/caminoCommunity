using Coco.Entities.Domain.Dbo;
using Coco.Entities.Enums;
using Coco.Entities.Dtos;
using Coco.Entities.Dtos.General;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IUserPhotoBusiness
    {
        Task<UserPhotoUpdateDto> UpdateUserPhotoAsync(UserPhotoUpdateDto model, long userId);
        Task DeleteUserPhotoAsync(long userId, UserPhotoTypeEnum userPhotoType);
        UserPhotoDto GetUserPhotoByCodeAsync(string code, UserPhotoTypeEnum type);
    }
}
