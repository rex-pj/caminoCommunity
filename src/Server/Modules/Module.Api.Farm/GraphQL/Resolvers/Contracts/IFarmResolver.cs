using Camino.Framework.Models;
using Camino.Core.Domain.Identities;
using Module.Api.Farm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.General;

namespace Module.Api.Farm.GraphQL.Resolvers.Contracts
{
    public interface IFarmResolver
    {
        Task<FarmModel> CreateFarmAsync(ApplicationUser currentUser, FarmModel criterias);
        Task<FarmModel> UpdateFarmAsync(ApplicationUser currentUser, FarmModel criterias);
        Task<FarmPageListModel> GetUserFarmsAsync(FarmFilterModel criterias);
        Task<FarmPageListModel> GetFarmsAsync(FarmFilterModel criterias);
        Task<IEnumerable<SelectOption>> SelectUserFarmsAsync(ApplicationUser currentUser, FarmSelectFilterModel criterias);
        Task<FarmModel> GetFarmAsync(FarmFilterModel criterias);
        Task<bool> DeleteFarmAsync(ApplicationUser currentUser, FarmFilterModel criterias);
    }
}
