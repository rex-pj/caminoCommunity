using Camino.Infrastructure.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Auth.Api.GraphQL.Resolvers.Contracts;
using Module.Auth.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Module.Auth.Api.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class UserPhotoQueries : BaseQueries
    {
        public async Task<UserAvatarModel> GetUserAvatar(ClaimsPrincipal claimsPrincipal, [Service] IUserPhotoResolver userResolver, FindUserModel criterias)
        {
            return await userResolver.GetUserAvatar(claimsPrincipal, criterias);
        }

        public async Task<IList<UserPhotoModel>> GetUserPhotos(ClaimsPrincipal claimsPrincipal, [Service] IUserPhotoResolver userResolver, FindUserModel criterias)
        {
            return await userResolver.GetUserPhotos(claimsPrincipal, criterias);
        }

        public async Task<UserCoverModel> GetUserCover(ClaimsPrincipal claimsPrincipal, [Service] IUserPhotoResolver userResolver, FindUserModel criterias)
        {
            return await userResolver.GetUserCover(claimsPrincipal, criterias);
        }
    }
}
