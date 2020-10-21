using Camino.Core.Models;
using Camino.Framework.Models;
using Module.Api.Farm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Farm.GraphQL.Resolvers.Contracts
{
    public interface IFarmResolver
    {
        Task<FarmModel> CreateFarmAsync(FarmModel criterias);
        Task<FarmPageListModel> GetUserFarmsAsync(FarmFilterModel criterias);
        Task<FarmPageListModel> GetFarmsAsync(FarmFilterModel criterias);
        Task<IEnumerable<ISelectOption>> SelectFarmsAsync(SelectFilterModel criterias);
    }
}
