using Camino.Service.Data.Filters;
using Camino.Service.Data.Identity;
using Camino.Service.Data.Page;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Identities.Contracts
{
    public interface ICountryBusiness
    {
        List<CountryResult> GetAll();
        Task<PageList<CountryResult>> GetAsync(CountryFilter filter);
        IList<CountryResult> Search(string query = "", int page = 1, int pageSize = 10);
        CountryResult Find(int id);
        CountryResult FindByName(string name);
        int Add(CountryResult countryDto);
        CountryResult Update(CountryResult countryDto);
    }
}
