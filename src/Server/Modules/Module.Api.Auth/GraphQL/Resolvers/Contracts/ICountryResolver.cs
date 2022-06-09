using Camino.Application.Contracts;
using System.Collections.Generic;

namespace Module.Api.Auth.GraphQL.Resolvers.Contracts
{
    public interface ICountryResolver
    {
        IEnumerable<SelectOption> GetSelections();
    }
}
