using Module.Api.Media.Models;
using Camino.Framework.Models;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Module.Api.Media.GraphQL.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<CommonResult> UpdateAvatarAsync(ClaimsPrincipal claimsPrincipal, UserPhotoUpdateModel criterias);
        Task<CommonResult> UpdateCoverAsync(ClaimsPrincipal claimsPrincipal, UserPhotoUpdateModel criterias);
        Task<CommonResult> DeleteAvatarAsync(ClaimsPrincipal claimsPrincipal, PhotoDeleteModel criterias);
        Task<CommonResult> DeleteCoverAsync(ClaimsPrincipal claimsPrincipal, PhotoDeleteModel criterias);
    }
}
