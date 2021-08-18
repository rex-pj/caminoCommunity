using Camino.Framework.Models;
using Camino.Core.Contracts.Services.Farms;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.General;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Configurations;
using Microsoft.Extensions.Options;

namespace Module.Api.Farm.GraphQL.Resolvers
{
    public class FarmTypeResolver : IFarmTypeResolver
    {
        private readonly IFarmTypeService _farmTypeService;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public FarmTypeResolver(IFarmTypeService farmTypeService, IOptions<PagerOptions> pagerOptions)
        {
            _farmTypeService = farmTypeService;
            _pagerOptions = pagerOptions.Value;
        }

        public async Task<IEnumerable<SelectOption>> GetFarmTypesAsync(BaseSelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new BaseSelectFilterModel();
            }

            var categories = await _farmTypeService.SearchAsync(new BaseFilter
            {
                Keyword = criterias.Query,
                PageSize = _pagerOptions.PageSize,
                Page = _defaultPageSelection
            });
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
