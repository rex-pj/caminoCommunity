using Camino.Core.Models;
using Camino.Framework.Models;
using System.Collections.Generic;

namespace Module.Api.Farm.GraphQL.Resolvers.Contracts
{
    public interface IFarmTypeResolver
    {
        IEnumerable<ISelectOption> GetFarmCategories(SelectFilterModel criterias);
    }
}
