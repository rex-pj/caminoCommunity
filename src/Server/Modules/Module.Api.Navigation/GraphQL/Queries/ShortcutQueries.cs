using Camino.Infrastructure.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Navigation.GraphQL.Resolvers.Contracts;
using Module.Api.Navigation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Navigation.GraphQL.Queries
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
