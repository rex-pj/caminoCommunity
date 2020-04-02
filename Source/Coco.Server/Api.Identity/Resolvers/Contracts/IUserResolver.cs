using Coco.Api.Framework.Models;
using Coco.Entities.Dtos.User;
using HotChocolate.Resolvers;
using System.Threading.Tasks;

namespace Api.Identity.Resolvers.Contracts
{
    public interface IUserResolver
    {
        Task<UpdatePerItemModel> UpdateUserInfoItemAsync(IResolverContext context);
        Task<IApiResult> SignoutAsync(IResolverContext context);
        Task<IApiResult> UpdateAvatarAsync(IResolverContext context);
        Task<IApiResult> UpdateCoverAsync(IResolverContext context);
        Task<IApiResult> DeleteAvatarAsync(IResolverContext context);
        Task<IApiResult> DeleteCoverAsync(IResolverContext context);
        Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(IResolverContext context);
        Task<UserTokenResult> UpdatePasswordAsync(IResolverContext context);
    }
}
