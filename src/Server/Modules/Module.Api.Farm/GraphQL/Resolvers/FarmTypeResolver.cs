using Camino.Framework.Models;
using Camino.Core.Contracts.Services.Farms;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.General;

namespace Module.Api.Farm.GraphQL.Resolvers
{
    public class FarmTypeResolver : IFarmTypeResolver
    {
        private readonly IFarmTypeService _farmTypeService;
        public FarmTypeResolver(IFarmTypeService farmTypeService)
        {
            _farmTypeService = farmTypeService;
        }

        public async Task<IEnumerable<SelectOption>> GetFarmTypesAsync(BaseSelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new BaseSelectFilterModel();
            }

            var categories = await _farmTypeService.SearchAsync(criterias.Query);
            if (categories == null || !categories.Any())
            {
                return new List<SelectOption>();
            }

            var categorySeletions = categories
                .Select(x => new SelectOption
                {
                    Id = x.Id.ToString(),
                    Text = x.Name
                });

            return categorySeletions;
        }
    }
}
