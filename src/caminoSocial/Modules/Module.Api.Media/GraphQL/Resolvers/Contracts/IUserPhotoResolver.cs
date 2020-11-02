using Module.Api.Media.Models;
using Camino.Framework.Models;
using System.Threading.Tasks;
using Camino.Service.Projections.Request;
using Camino.IdentityManager.Models;

namespace Module.Api.Media.GraphQL.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<CommonResult> UpdateAvatarAsync(ApplicationUser currentUser, UserPhotoUpdateRequest criterias);
        Task<CommonResult> UpdateCoverAsync(ApplicationUser currentUser, UserPhotoUpdateRequest criterias);
        Task<CommonResult> DeleteAvatarAsync(ApplicationUser currentUser, PhotoDeleteModel criterias);
        Task<CommonResult> DeleteCoverAsync(ApplicationUser currentUser, PhotoDeleteModel criterias);
    }
}
