using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Media.GraphQL.Resolvers.Contracts;
using Module.Api.Media.Models;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Module.Api.Media.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class UserPhotoMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<CommonResult> UpdateAvatarAsync(ClaimsPrincipal claimsPrincipal, [Service] IUserPhotoResolver userPhotoResolver, UserPhotoUpdateModel criterias)
        {
            return await userPhotoResolver.UpdateAvatarAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<CommonResult> UpdateCoverAsync(ClaimsPrincipal claimsPrincipal, [Service] IUserPhotoResolver userPhotoResolver, UserPhotoUpdateModel criterias)
        {
            return await userPhotoResolver.UpdateCoverAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<CommonResult> DeleteAvatarAsync(ClaimsPrincipal claimsPrincipal, [Service] IUserPhotoResolver userPhotoResolver, PhotoDeleteModel criterias)
        {
            return await userPhotoResolver.DeleteAvatarAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<CommonResult> DeleteCoverAsync(ClaimsPrincipal claimsPrincipal, [Service] IUserPhotoResolver userPhotoResolver, PhotoDeleteModel criterias)
        {
            return await userPhotoResolver.DeleteCoverAsync(claimsPrincipal, criterias);
        }
    }
}
