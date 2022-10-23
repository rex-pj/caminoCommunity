using Camino.Infrastructure.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using Module.Api.Farm.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Api.Farm.GraphQL.Queries
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
