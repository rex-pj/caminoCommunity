using Camino.Framework.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using Module.Api.Farm.Models;
using System.Threading.Tasks;

namespace Module.Api.Farm.GraphQL.Queries
{
    [ExtendObjectType(Name = "Query")]
    public class FarmQueries : BaseQueries
    {
        public async Task<FarmPageListModel> GetUserFarmsAsync([Service] IFarmResolver farmResolver, FarmFilterModel criterias)
        {
            return await farmResolver.GetUserFarmsAsync(criterias);
        }

        public async Task<FarmPageListModel> GetFarmsAsync([Service] IFarmResolver farmResolver, FarmFilterModel criterias)
        {
            return await farmResolver.GetFarmsAsync(criterias);
        }

        public async Task<FarmModel> GetFarmAsync([Service] IFarmResolver farmResolver, FarmFilterModel criterias)
        {
            return await farmResolver.GetFarmAsync(criterias);
        }
    }
}
