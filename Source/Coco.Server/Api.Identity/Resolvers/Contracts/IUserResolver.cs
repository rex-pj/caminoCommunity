using Coco.Api.Framework.Models;
using Coco.Entities.Dtos.General;
using GraphQL.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Identity.Resolvers.Contracts
{
    public interface IUserResolver
    {
        Task<ApiResult> UpdateUserInfoItemAsync(ResolveFieldContext<object> context);
        Task<ApiResult> SignoutAsync(IDictionary<string, object> userContext);
        Task<ApiResult> UpdateAvatarAsync(ResolveFieldContext<object> context);
        Task<ApiResult> UpdateCoverAsync(ResolveFieldContext<object> context);
        Task<ApiResult> DeleteAvatarAsync(ResolveFieldContext<object> context);
        Task<ApiResult> DeleteCoverAsync(ResolveFieldContext<object> context);
        Task<ApiResult> UpdateIdentifierAsync(ResolveFieldContext<object> context);
        Task<ApiResult> UpdatePasswordAsync(ResolveFieldContext<object> context);
    }
}
