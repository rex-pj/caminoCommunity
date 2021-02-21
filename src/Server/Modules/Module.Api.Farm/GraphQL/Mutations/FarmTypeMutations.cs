using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using Camino.Framework.Models;
using Camino.Shared.General;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Farm.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class FarmTypeMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<IEnumerable<SelectOption>> GetFarmTypesAsync([Service] IFarmTypeResolver farmResolver, BaseSelectFilterModel criterias)
        {
            return await farmResolver.GetFarmTypesAsync(criterias);
        }
    }
}
