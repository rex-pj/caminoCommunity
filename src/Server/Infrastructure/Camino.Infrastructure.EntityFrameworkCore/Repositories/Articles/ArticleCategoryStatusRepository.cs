using Camino.Shared.Requests.Filters;
using System.Collections.Generic;
using System.Linq;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Camino.Shared.General;
using Camino.Core.Contracts.Repositories.Articles;
using Camino.Core.Contracts.DependencyInjection;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Articles
{
    public class ArticleCategoryStatusRepository : IArticleCategoryStatusRepository, IScopedDependency
    {
        public ArticleCategoryStatusRepository()
        {
        }

        public IList<SelectOption> Search(IdRequestFilter<int?> filter, string search = "")
        {
            search = search != null ? search.ToLower() : "";
            var result = new List<SelectOption>();
            if (filter.Id.HasValue)
            {
                var selected = (ArticleCategoryStatus)filter.Id;
                result = EnumUtil.ToSelectOptions(selected).ToList();
            }
            else
            {
                result = EnumUtil.ToSelectOptions<ArticleCategoryStatus>().ToList();
            }

            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
            return result;
        }
    }
}
