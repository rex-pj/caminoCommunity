using Camino.Shared.Requests.Filters;
using Camino.Shared.Requests.Identifiers;
using Camino.Shared.Results.Identifiers;
using Camino.Shared.Results.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Services.Identities
{
    public interface ICountryService
    {
        List<CountryResult> Get();
        Task<BasePageList<CountryResult>> GetAsync(CountryFilter filter);
        IList<CountryResult> Search(BaseFilter filter);
        CountryResult Find(int id);
        CountryResult FindByName(string name);
        Task<int> CreateAsync(CountryModifyRequest request);
        Task<bool> UpdateAsync(CountryModifyRequest request);
    }
}
