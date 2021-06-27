using Camino.Shared.General;
using Camino.Shared.Requests.Filters;
using System.Collections.Generic;

namespace Camino.Core.Contracts.Repositories.Products
{
    public interface IProductCategoryStatusRepository
    {
        IList<SelectOption> Search(IdRequestFilter<int?> filter, string search = "");
    }
}
