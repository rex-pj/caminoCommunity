using Camino.Business.Contracts;
using Camino.Data.Contracts;
using Camino.Data.Entities.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Camino.Business.Implementation
{
    public class CountryBusiness : ICountryBusiness
    {
        private readonly IRepository<Country> _countryRepository;

        public CountryBusiness(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public List<Country> GetAll()
        {
            return _countryRepository.Get().ToList();
        }
    }
}
