using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using Camino.Framework.Models;
using Camino.Core.Domain.Identities;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Media.GraphQL.Resolvers.Contracts;
using Module.Api.Media.Models;
using System.Threading.Tasks;
using Camino.Shared.Requests.Identifiers;

namespace Module.Api.Media.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class UserPhotoMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<CommonResult> UpdateAvatarAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IUserPhotoResolver userPhotoResolver, UserPhotoUpdateRequest criterias)
        {
            return await userPhotoResolver.UpdateAvatarAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<CommonResult> UpdateCoverAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IUserPhotoResolver userPhotoResolver, UserPhotoUpdateRequest criterias)
        {
            return await userPhotoResolver.UpdateCoverAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<CommonResult> DeleteAvatarAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IUserPhotoResolver userPhotoResolver, PhotoDeleteModel criterias)
        {
            return await userPhotoResolver.DeleteAvatarAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<CommonResult> DeleteCoverAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IUserPhotoResolver userPhotoResolver, PhotoDeleteModel criterias)
        {
            return await userPhotoResolver.DeleteCoverAsync(currentUser, criterias);
        }
    }
}
