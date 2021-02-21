using Camino.Framework.Models;
using Camino.Shared.General;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Farm.GraphQL.Resolvers.Contracts
{
    public interface IFarmTypeResolver
    {
        Task<IEnumerable<SelectOption>> GetFarmTypesAsync(BaseSelectFilterModel criterias);
    }
}
