using Camino.Core.Domain.Identities;
using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using Module.Api.Farm.Models;
using System.Threading.Tasks;

namespace Module.Api.Farm.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class FarmQueries : BaseQueries
    {
        public async Task<FarmPageListModel> GetUserFarmsAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IFarmResolver farmResolver, FarmFilterModel criterias)
        {
            return await farmResolver.GetUserFarmsAsync(currentUser, criterias);
        }

        public async Task<FarmPageListModel> GetFarmsAsync([Service] IFarmResolver farmResolver, FarmFilterModel criterias)
        {
            return await farmResolver.GetFarmsAsync(criterias);
        }

        public async Task<FarmModel> GetFarmAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IFarmResolver farmResolver, FarmFilterModel criterias)
        {
            return await farmResolver.GetFarmAsync(currentUser, criterias);
        }
    }
}
