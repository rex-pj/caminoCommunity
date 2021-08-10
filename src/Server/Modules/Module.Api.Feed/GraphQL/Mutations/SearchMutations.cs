using Camino.Framework.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Feed.GraphQL.Resolvers.Contracts;
using Module.Api.Feed.Models;
using System.Threading.Tasks;

namespace Module.Api.Feed.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class SearchMutations : BaseMutations
    {
        public async Task<SearchInGroupResultModel> LiveSearchAsync([Service] ISearchResolver farmResolver, FeedFilterModel criterias)
        {
            return await farmResolver.LiveSearchAsync(criterias);
        }
    }
}
