using Module.Api.Content.Models;
using Camino.Business.Dtos.General;
using Camino.Framework.Models;
using System.Threading.Tasks;

namespace Module.Api.Content.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<ICommonResult> UpdateAvatarAsync(UserPhotoUpdation criterias);
        Task<ICommonResult> UpdateCoverAsync(UserPhotoUpdation criterias);
        Task<ICommonResult> DeleteAvatarAsync(PhotoDeleteModel criterias);
        Task<ICommonResult> DeleteCoverAsync(PhotoDeleteModel criterias);
    }
}
