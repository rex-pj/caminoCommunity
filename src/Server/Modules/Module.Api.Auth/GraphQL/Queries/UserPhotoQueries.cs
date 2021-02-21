using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Queries;
using Camino.Core.Domain.Identities;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Auth.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class UserPhotoQueries : BaseQueries
    {
        public async Task<UserAvatarModel> GetUserAvatar([ApplicationUserState] ApplicationUser currentUser, [Service] IUserPhotoResolver userResolver, FindUserModel criterias)
        {
            return await userResolver.GetUserAvatar(currentUser, criterias);
        }

        public async Task<IList<UserPhotoModel>> GetUserPhotos([ApplicationUserState] ApplicationUser currentUser, [Service] IUserPhotoResolver userResolver, FindUserModel criterias)
        {
            return await userResolver.GetUserPhotos(currentUser, criterias);
        }

        public async Task<UserCoverModel> GetUserCover([ApplicationUserState] ApplicationUser currentUser, [Service] IUserPhotoResolver userResolver, FindUserModel criterias)
        {
            return await userResolver.GetUserCover(currentUser, criterias);
        }
    }
}
