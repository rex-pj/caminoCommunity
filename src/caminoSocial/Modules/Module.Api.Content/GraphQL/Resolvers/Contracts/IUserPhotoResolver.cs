using Module.Api.Content.Models;
using Camino.Framework.Models;
using System.Threading.Tasks;
using Camino.Service.Projections.Request;

namespace Module.Api.Content.GraphQL.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<ICommonResult> UpdateAvatarAsync(UserPhotoUpdateRequest criterias);
        Task<ICommonResult> UpdateCoverAsync(UserPhotoUpdateRequest criterias);
        Task<ICommonResult> DeleteAvatarAsync(PhotoDeleteModel criterias);
        Task<ICommonResult> DeleteCoverAsync(PhotoDeleteModel criterias);
    }
}
