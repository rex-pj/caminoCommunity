using Camino.Infrastructure.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Farm.Api.GraphQL.Resolvers.Contracts;
using Module.Farm.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Farm.Api.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class FarmQueries : BaseQueries
    {
        public async Task<FarmPageListModel> GetUserFarmsAsync(ClaimsPrincipal claimsPrincipal, [Service] IFarmResolver farmResolver, FarmFilterModel criterias)
        {
            return await farmResolver.GetUserFarmsAsync(claimsPrincipal, criterias);
        }

        public async Task<FarmPageListModel> GetFarmsAsync([Service] IFarmResolver farmResolver, FarmFilterModel criterias)
        {
            return await farmResolver.GetFarmsAsync(criterias);
        }

        public async Task<FarmModel> GetFarmAsync(ClaimsPrincipal claimsPrincipal, [Service] IFarmResolver farmResolver, FarmIdFilterModel criterias)
        {
            return await farmResolver.GetFarmAsync(claimsPrincipal, criterias);
        }
    }
}
