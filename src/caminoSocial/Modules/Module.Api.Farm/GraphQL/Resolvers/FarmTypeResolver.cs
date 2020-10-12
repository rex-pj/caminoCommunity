using Camino.Core.Models;
using Camino.Framework.Models;
using Camino.Service.Business.Farms.Contracts;
using Camino.Service.Projections.Content;
using Module.Api.Farm.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Module.Api.Farm.GraphQL.Resolvers
{
    public class FarmTypeResolver : IFarmTypeResolver
    {
        private readonly IFarmTypeBusiness _farmTypeBusiness;
        public FarmTypeResolver(IFarmTypeBusiness farmTypeBusiness)
        {
            _farmTypeBusiness = farmTypeBusiness;
        }

        public IEnumerable<ISelectOption> GetFarmCategories(SelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new SelectFilterModel();
            }

            var categories = _farmTypeBusiness.Search(criterias.Query);
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
