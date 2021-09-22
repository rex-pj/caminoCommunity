using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using Module.Api.Farm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.General;
using System.Security.Claims;

namespace Module.Api.Farm.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class FarmMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<FarmIdResultModel> CreateFarmAsync(ClaimsPrincipal claimsPrincipal, [Service] IFarmResolver farmResolver, CreateFarmModel criterias)
        {
            return await farmResolver.CreateFarmAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<FarmIdResultModel> UpdateFarmAsync(ClaimsPrincipal claimsPrincipal, [Service] IFarmResolver farmResolver, UpdateFarmModel criterias)
        {
            return await farmResolver.UpdateFarmAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<IEnumerable<SelectOption>> SelectUserFarmsAsync(ClaimsPrincipal claimsPrincipal, [Service] IFarmResolver farmResolver, FarmSelectFilterModel criterias)
        {
            return await farmResolver.SelectUserFarmsAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<bool> DeleteFarmAsync(ClaimsPrincipal claimsPrincipal, [Service] IFarmResolver farmResolver, FarmIdFilterModel criterias)
        {
            return await farmResolver.DeleteFarmAsync(claimsPrincipal, criterias);
        }
    }
}
