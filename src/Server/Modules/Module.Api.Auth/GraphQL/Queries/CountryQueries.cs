using Camino.Application.Contracts;
using Camino.Infrastructure.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;

namespace Module.Api.Auth.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class CountryQueries : BaseQueries
    {
        public IEnumerable<SelectOption> GetCountrySelections([Service] ICountryResolver countryResolver)
        {
            return countryResolver.GetSelections();
        }
    }
}
