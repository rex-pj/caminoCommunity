using Module.Api.Media.Models;
using Camino.Framework.Models;
using System.Threading.Tasks;
using Camino.Core.Domain.Identities;

namespace Module.Api.Media.GraphQL.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<CommonResult> UpdateAvatarAsync(ApplicationUser currentUser, UserPhotoUpdateModel criterias);
        Task<CommonResult> UpdateCoverAsync(ApplicationUser currentUser, UserPhotoUpdateModel criterias);
        Task<CommonResult> DeleteAvatarAsync(ApplicationUser currentUser, PhotoDeleteModel criterias);
        Task<CommonResult> DeleteCoverAsync(ApplicationUser currentUser, PhotoDeleteModel criterias);
    }
}
