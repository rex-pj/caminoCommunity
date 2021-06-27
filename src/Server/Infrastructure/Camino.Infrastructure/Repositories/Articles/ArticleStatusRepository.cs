using Camino.Shared.Requests.Filters;
using System.Collections.Generic;
using System.Linq;
using Camino.Core.Contracts.Repositories.Articles;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Camino.Shared.General;

namespace Camino.Infrastructure.Repositories.Articles
{
    public class ArticleStatusRepository : IArticleStatusRepository
    {
        public ArticleStatusRepository()
        {
        }

        public IList<SelectOption> Search(IdRequestFilter<int?> filter, string search = "")
        {
            search = search != null ? search.ToLower() : "";
            var result = new List<SelectOption>();
            if (filter.Id.HasValue)
            {
                var selected = (ArticleStatus)filter.Id;
                result = EnumUtil.ToSelectOptions(selected).ToList();
            }
            else
            {
                result = EnumUtil.ToSelectOptions<ArticleStatus>().ToList();
            }

            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
            return result;
        }
    }
}
