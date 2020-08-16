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

        public CountryDto Find(int id)
        {
            var country = _countryRepository.Get(x => x.Id == id)
                .Select(x => new CountryDto()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefault();

            return country;
        }

        public CountryDto FindByName(string name)
        {
            var country = _countryRepository.Get(x => x.Name == name)
                .Select(x => new CountryDto()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefault();

            return country;
        }

        public int Add(CountryDto countryDto)
        {
            var country = new Country()
            {
                Code = countryDto.Code,
                Name = countryDto.Name
            };

            var id = _countryRepository.AddWithInt32Entity(country);
            return id;
        }

        public CountryDto Update(CountryDto countryDto)
        {
            var exist = _countryRepository.FirstOrDefault(x => x.Id == countryDto.Id);
            if (exist == null)
            {
                return null;
            }
            exist.Code = countryDto.Code;
            exist.Name = countryDto.Name;

            _countryRepository.Update(exist);
            return countryDto;
        }
    }
}
