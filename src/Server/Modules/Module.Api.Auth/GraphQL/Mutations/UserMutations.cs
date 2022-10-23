using Camino.Infrastructure.GraphQL.Attributes;
using Camino.Infrastructure.GraphQL.Mutations;
using Camino.Infrastructure.AspNetCore.Models;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Application.Contracts;

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

        public async Task<IEnumerable<SelectOption>> SelectUsersAsync([Service] IUserResolver userResolver, UserFilterModel criterias)
        {
            return await userResolver.SelectUsersAsync(criterias);
        }
    }
}
