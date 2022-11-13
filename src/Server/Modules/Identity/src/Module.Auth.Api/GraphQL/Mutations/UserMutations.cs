using Camino.Infrastructure.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Types;
using Module.Auth.Api.GraphQL.Resolvers.Contracts;
using Module.Auth.Api.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Application.Contracts;

namespace Module.Auth.Api.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class UserMutations : BaseMutations
    {
        public async Task<IEnumerable<SelectOption>> SelectUsersAsync([Service] IUserResolver userResolver, UserFilterModel criterias)
        {
            return await userResolver.SelectUsersAsync(criterias);
        }
    }
}
