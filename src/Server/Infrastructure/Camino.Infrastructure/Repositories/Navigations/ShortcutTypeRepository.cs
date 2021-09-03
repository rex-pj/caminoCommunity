using Camino.Shared.Requests.Filters;
using System.Collections.Generic;
using System.Linq;
using Camino.Core.Utils;
using Camino.Shared.General;
using Camino.Core.Contracts.Repositories.Navigations;
using Camino.Infrastructure.Commons.Enums;

namespace Camino.Infrastructure.Repositories.Navigations
{
    public class ShortcutTypeRepository : IShortcutTypeRepository
    {
        public ShortcutTypeRepository()
        {
        }

        public IList<SelectOption> Get(ShortcutTypeFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var result = new List<SelectOption>();
            if (filter.Id > 0)
            {
                var selected = (ShortcutType)filter.Id;
                result = EnumUtil.ToSelectOptions(selected).ToList();
            }
            else
            {
                result = EnumUtil.ToSelectOptions<ShortcutType>().ToList();
            }

            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
            return result;
        }
    }
}
