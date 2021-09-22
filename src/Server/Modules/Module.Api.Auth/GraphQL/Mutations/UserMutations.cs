using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using System.Threading.Tasks;
using Camino.Shared.Requests.Authentication;
using System.Collections.Generic;
using Camino.Shared.General;
using System.Security.Claims;

namespace Module.Api.Auth.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class UserMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<PartialUpdateResultModel> PartialUserUpdateAsync(ClaimsPrincipal claimsPrincipal, [Service] IUserResolver userResolver, PartialUpdateRequestModel criterias)
        {
            return await userResolver.PartialUserUpdateAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync(ClaimsPrincipal claimsPrincipal, [Service] IUserResolver userResolver, UserIdentifierUpdateModel criterias)
        {
            return await userResolver.UpdateIdentifierAsync(claimsPrincipal, criterias);
        }

        public async Task<CommonResult> SignupAsync([Service] IUserResolver userResolver, SignupModel criterias)
        {
            return await userResolver.SignupAsync(criterias);
        }

        public async Task<IEnumerable<SelectOption>> SelectUsersAsync([Service] IUserResolver userResolver, UserFilterModel criterias)
        {
            return await userResolver.SelectUsersAsync(criterias);
        }
    }
}
