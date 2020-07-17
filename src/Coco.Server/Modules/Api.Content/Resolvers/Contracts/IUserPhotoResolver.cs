using Api.Content.Models;
using Coco.Core.Dtos.General;
using Coco.Framework.Models;
using System.Threading.Tasks;

namespace Api.Content.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<ICommonResult> UpdateAvatarAsync(UserPhotoUpdateDto criterias);
        Task<ICommonResult> UpdateCoverAsync(UserPhotoUpdateDto criterias);
        Task<ICommonResult> DeleteAvatarAsync(PhotoDeleteModel criterias);
        Task<ICommonResult> DeleteCoverAsync(PhotoDeleteModel criterias);
    }
}
