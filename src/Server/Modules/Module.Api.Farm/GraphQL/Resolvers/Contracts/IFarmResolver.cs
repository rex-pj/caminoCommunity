﻿using Module.Api.Farm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.General;
using System.Security.Claims;

namespace Module.Api.Farm.GraphQL.Resolvers.Contracts
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
