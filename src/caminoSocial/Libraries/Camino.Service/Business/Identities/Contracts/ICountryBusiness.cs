using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Identity;
using Camino.Service.Projections.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Identities.Contracts
{
    public interface ICountryBusiness
    {
        List<CountryProjection> Get();
        Task<BasePageList<CountryProjection>> GetAsync(CountryFilter filter);
        IList<CountryProjection> Search(string query = "", int page = 1, int pageSize = 10);
        CountryProjection Find(int id);
        CountryProjection FindByName(string name);
        int Create(CountryProjection countryRequest);
        CountryProjection Update(CountryProjection countryRequest);
    }
}
