using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using Camino.Framework.Models;
using Camino.Core.Domain.Identities;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using Module.Api.Farm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.General;

namespace Module.Api.Farm.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class FarmMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<FarmModel> CreateFarmAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IFarmResolver farmResolver, FarmModel criterias)
        {
            return await farmResolver.CreateFarmAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<FarmModel> UpdateFarmAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IFarmResolver farmResolver, FarmModel criterias)
        {
            return await farmResolver.UpdateFarmAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<IEnumerable<SelectOption>> SelectUserFarmsAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IFarmResolver farmResolver, FarmSelectFilterModel criterias)
        {
            return await farmResolver.SelectUserFarmsAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<bool> DeleteFarmAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IFarmResolver farmResolver, FarmFilterModel criterias)
        {
            return await farmResolver.DeleteFarmAsync(currentUser, criterias);
        }
    }
}
