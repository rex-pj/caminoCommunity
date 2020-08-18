using Camino.Business.Dtos.General;
using Camino.Business.Dtos.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Business.Contracts
{
    public interface ICountryBusiness
    {
        List<CountryDto> GetAll();
        Task<PageListDto<CountryDto>> GetAsync(CountryFilterDto filter);
        IList<CountryDto> Search(string query = "", int page = 1, int pageSize = 10);
        CountryDto Find(int id);
        CountryDto FindByName(string name);
        int Add(CountryDto countryDto);
        CountryDto Update(CountryDto countryDto);
    }
}
