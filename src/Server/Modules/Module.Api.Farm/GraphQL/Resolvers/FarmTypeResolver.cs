using Camino.Framework.Models;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Camino.Application.Contracts.AppServices.Farms;
using Camino.Shared.Configuration.Options;
using Camino.Application.Contracts;

namespace Module.Api.Farm.GraphQL.Resolvers
{
    public class FarmTypeResolver : IFarmTypeResolver
    {
        private readonly IFarmTypeAppService _farmTypeAppService;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public FarmTypeResolver(IFarmTypeAppService farmTypeAppService, IOptions<PagerOptions> pagerOptions)
        {
            _farmTypeAppService = farmTypeAppService;
            _pagerOptions = pagerOptions.Value;
        }

        public async Task<IEnumerable<SelectOption>> GetFarmTypesAsync(BaseSelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new BaseSelectFilterModel();
            }

            var categories = await _farmTypeAppService.SearchAsync(new BaseFilter
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
