using Camino.Business.Contracts;
using Camino.Business.Dtos.Identity;
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

        public List<CountryDto> GetAll()
        {
            return _countryRepository.Get()
                .Select(x => new CountryDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code
                })
                .ToList();
        }

        public IList<CountryDto> Search(string query = "", int page = 1, int pageSize = 10)
        {
            if (query == null)
            {
                query = string.Empty;
            }

            query = query.ToLower();

            var data = _countryRepository.Get(x => string.IsNullOrEmpty(query) || x.Name.ToLower().Contains(query)
                || x.Code.ToLower().Contains(query));

            if (pageSize > 0)
            {
                data = data.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var countries = data
                .Select(x => new CountryDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code
                })
                .ToList();

            return countries;
        }
    }
}
