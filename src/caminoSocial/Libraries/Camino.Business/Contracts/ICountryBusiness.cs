using Camino.Business.Dtos.Identity;
using Camino.Data.Entities.Identity;
using System.Collections.Generic;

namespace Camino.Business.Contracts
{
    public interface ICountryBusiness
    {
        List<CountryDto> GetAll();
        IList<CountryDto> Search(string query = "", int page = 1, int pageSize = 10);
    }
}
