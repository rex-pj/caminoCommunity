using Camino.Framework.GraphQL.Queries;
using Camino.Shared.General;
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
