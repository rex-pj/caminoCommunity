using Coco.Entities.Domain.Dbo;
using Coco.Entities.Model.General;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IUserPhotoBusiness
    {
        Task<UpdateAvatarModel> UpdateAvatarAsync(UpdateAvatarModel model, long userId);
        Task<UserPhoto> GetAvatarByIdAsync(long id);
        Task<UserPhoto> GetAvatarByCodeAsync(string code);
    }
}
