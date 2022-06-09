using Camino.Application.Contracts;
using Camino.Framework.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Farm.GraphQL.Resolvers.Contracts
{
    public interface IFarmTypeResolver
    {
        Task<IEnumerable<SelectOption>> GetFarmTypesAsync(BaseSelectFilterModel criterias);
    }
}
