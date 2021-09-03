using Module.Api.Farm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.General;
using System.Security.Claims;

namespace Module.Api.Farm.GraphQL.Resolvers.Contracts
{
    public interface IFarmResolver
    {
        Task<FarmModel> CreateFarmAsync(ClaimsPrincipal claimsPrincipal, FarmModel criterias);
        Task<FarmModel> UpdateFarmAsync(ClaimsPrincipal claimsPrincipal, FarmModel criterias);
        Task<FarmPageListModel> GetUserFarmsAsync(ClaimsPrincipal claimsPrincipal, FarmFilterModel criterias);
        Task<FarmPageListModel> GetFarmsAsync(FarmFilterModel criterias);
        Task<IEnumerable<SelectOption>> SelectUserFarmsAsync(ClaimsPrincipal claimsPrincipal, FarmSelectFilterModel criterias);
        Task<FarmModel> GetFarmAsync(ClaimsPrincipal claimsPrincipal, FarmFilterModel criterias);
        Task<bool> DeleteFarmAsync(ClaimsPrincipal claimsPrincipal, FarmFilterModel criterias);
    }
}
