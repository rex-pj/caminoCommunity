using Camino.Business.Contracts;
using Camino.Business.Dtos.General;
using Camino.Business.Dtos.Identity;
using Camino.Data.Contracts;
using Camino.Data.Entities.Identity;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<PageListDto<CountryDto>> GetAsync(CountryFilterDto filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var query = _countryRepository.Table
                .Select(x => new CountryDto()
                {
                    Code = x.Code,
                    Id = x.Id,
                    Name = x.Name
                });

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Code.ToLower().Contains(search) || x.Name.ToLower().Contains(search));
            }

            var filteredNumber = query.Select(x => x.Id).Count();

            var countries = await query.OrderBy(x => x.Code).Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize)
                                         .ToListAsync();

            var result = new PageListDto<CountryDto>(countries)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
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
