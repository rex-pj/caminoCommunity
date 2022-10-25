using Camino.Infrastructure.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Navigation.Api.GraphQL.Resolvers.Contracts;
using Module.Navigation.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Navigation.Api.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class ShortcutQueries : BaseQueries
    {
        public async Task<IList<ShortcutModel>> GetShortcutsAsync([Service] IShortcutResolver articleResolver, ShortcutFilterModel criterias)
        {
            return await articleResolver.GetShortcutsAsync(criterias);
        }
    }
}
