using Coco.Framework.Models;
using HotChocolate.Resolvers;
using System.Threading.Tasks;

namespace Api.Content.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<ICommonResult> UpdateAvatarAsync(IResolverContext context);
        Task<ICommonResult> UpdateCoverAsync(IResolverContext context);
        Task<ICommonResult> DeleteAvatarAsync(IResolverContext context);
        Task<ICommonResult> DeleteCoverAsync(IResolverContext context);
    }
}
