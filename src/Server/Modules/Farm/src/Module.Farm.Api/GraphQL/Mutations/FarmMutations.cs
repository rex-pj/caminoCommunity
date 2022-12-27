using Camino.Infrastructure.GraphQL.Attributes;
using Camino.Infrastructure.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Types;
using Module.Farm.Api.GraphQL.Resolvers.Contracts;
using Module.Farm.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Camino.Application.Contracts;

namespace Module.Farm.Api.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class FarmMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<IEnumerable<SelectOption>> SelectUserFarmsAsync(ClaimsPrincipal claimsPrincipal, [Service] IFarmResolver farmResolver, FarmSelectFilterModel criterias)
        {
            return await farmResolver.SelectUserFarmsAsync(claimsPrincipal, criterias);
        }
    }
}
