using Camino.Shared.General;
using Camino.Shared.Requests.Filters;
using System.Collections.Generic;

namespace Camino.Core.Contracts.Repositories.Farms
{
    public interface IFarmStatusRepository
    {
        IList<SelectOption> Search(IdRequestFilter<int?> filter, string search = "");
    }
}
