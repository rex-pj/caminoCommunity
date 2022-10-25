using Module.Farm.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Camino.Application.Contracts;

namespace Module.Farm.Api.GraphQL.Resolvers.Contracts
{
    public interface IFarmResolver
    {
        Task<FarmIdResultModel> CreateFarmAsync(ClaimsPrincipal claimsPrincipal, CreateFarmModel criterias);
        Task<FarmIdResultModel> UpdateFarmAsync(ClaimsPrincipal claimsPrincipal, UpdateFarmModel criterias);
        Task<FarmPageListModel> GetUserFarmsAsync(ClaimsPrincipal claimsPrincipal, FarmFilterModel criterias);
        Task<FarmPageListModel> GetFarmsAsync(FarmFilterModel criterias);
        Task<IEnumerable<SelectOption>> SelectUserFarmsAsync(ClaimsPrincipal claimsPrincipal, FarmSelectFilterModel criterias);
        Task<FarmModel> GetFarmAsync(ClaimsPrincipal claimsPrincipal, FarmIdFilterModel criterias);
        Task<bool> DeleteFarmAsync(ClaimsPrincipal claimsPrincipal, FarmIdFilterModel criterias);
    }
}
