using Camino.Service.Data.Filters;
using Camino.Service.Data.Identity;
using Camino.Service.Data.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Identities.Contracts
{
    public interface ICountryBusiness
    {
        List<CountryProjection> GetAll();
        Task<BasePageList<CountryProjection>> GetAsync(CountryFilter filter);
        IList<CountryProjection> Search(string query = "", int page = 1, int pageSize = 10);
        CountryProjection Find(int id);
        CountryProjection FindByName(string name);
        int Add(CountryProjection countryRequest);
        CountryProjection Update(CountryProjection countryRequest);
    }
}
