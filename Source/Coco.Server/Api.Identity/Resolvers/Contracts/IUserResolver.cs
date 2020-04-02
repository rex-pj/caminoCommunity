using Coco.Api.Framework.Models;
using HotChocolate.Resolvers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Identity.Resolvers.Contracts
{
    public interface IUserResolver
    {
        Task<ApiResult> UpdateUserInfoItemAsync(IResolverContext context);
        Task<ApiResult> SignoutAsync(IDictionary<string, object> userContext);
        Task<ApiResult> UpdateAvatarAsync(IResolverContext context);
        Task<ApiResult> UpdateCoverAsync(IResolverContext context);
        Task<ApiResult> DeleteAvatarAsync(IResolverContext context);
        Task<ApiResult> DeleteCoverAsync(IResolverContext context);
        Task<ApiResult> UpdateIdentifierAsync(IResolverContext context);
        Task<ApiResult> UpdatePasswordAsync(IResolverContext context);
    }
}
