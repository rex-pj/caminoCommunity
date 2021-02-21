using Camino.Shared.General;
using System.Collections.Generic;

namespace Module.Api.Auth.GraphQL.Resolvers.Contracts
{
    public interface IGenderResolver
    {
        IEnumerable<SelectOption> GetSelections();
    }
}
