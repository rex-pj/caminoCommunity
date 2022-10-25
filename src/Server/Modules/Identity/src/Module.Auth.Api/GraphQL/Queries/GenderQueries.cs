using Camino.Application.Contracts;
using Camino.Infrastructure.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Auth.Api.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;

namespace Module.Auth.Api.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class GenderQueries : BaseQueries
    {
        public IEnumerable<SelectOption> GetGenderSelections([Service] IGenderResolver genderResolver)
        {
            return genderResolver.GetSelections();
        }
    }
}
