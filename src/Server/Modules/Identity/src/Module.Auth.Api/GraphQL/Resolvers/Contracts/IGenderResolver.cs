using Camino.Application.Contracts;
using System.Collections.Generic;

namespace Module.Auth.Api.GraphQL.Resolvers.Contracts
{
    public interface IGenderResolver
    {
        IEnumerable<SelectOption> GetSelections();
    }
}
