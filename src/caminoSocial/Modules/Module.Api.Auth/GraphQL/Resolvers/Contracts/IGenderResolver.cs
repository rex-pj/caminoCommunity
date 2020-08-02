using Camino.Core.Models;
using System.Collections.Generic;

namespace Module.Api.Auth.GraphQL.Resolvers.Contracts
{
    public interface IGenderResolver
    {
        IEnumerable<ISelectOption> GetSelections();
    }
}
