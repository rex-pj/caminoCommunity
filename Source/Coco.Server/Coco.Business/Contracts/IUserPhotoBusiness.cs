using Coco.Entities.Domain.Dbo;
using Coco.Entities.Enums;
using Coco.Entities.Model;
using Coco.Entities.Model.General;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IUserPhotoBusiness
    {
        Task<UpdateAvatarModel> UpdateAvatarAsync(UpdateAvatarModel model, long userId);
        Task<UserPhoto> GetAvatarByIdAsync(long id);
        Task DeleteAvatarAsync(long userId);
        UserPhotoModel GetUserPhotoByCodeAsync(string code, UserPhotoTypeEnum type);
    }
}
