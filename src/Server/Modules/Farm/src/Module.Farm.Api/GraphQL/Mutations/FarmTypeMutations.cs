using Camino.Application.Contracts;
using Camino.Infrastructure.AspNetCore.Models;
using Camino.Infrastructure.GraphQL.Attributes;
using Camino.Infrastructure.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Types;
using Module.Farm.Api.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Farm.Api.GraphQL.Mutations
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
