using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Identifiers;
using Camino.Core.Contracts.Data;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Core.Contracts.Repositories.Identities;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Requests.Identifiers;

namespace Camino.Infrastructure.Repositories.Identifiers
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IRepository<Country> _countryRepository;

        public CountryRepository(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public List<CountryResult> Get()
        {
            return _countryRepository.Get()
                .Select(x => new CountryResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code
                })
                .ToList();
        }

        public async Task<BasePageList<CountryResult>> GetAsync(CountryFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var query = _countryRepository.Table
                .Select(x => new CountryResult()
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

            var result = new BasePageList<CountryResult>(countries)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public IList<CountryResult> Search(string query = "", int page = 1, int pageSize = 10)
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
                .Select(x => new CountryResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code
                })
                .ToList();

            return countries;
        }

        public CountryResult Find(int id)
        {
            var country = _countryRepository.Get(x => x.Id == id)
                .Select(x => new CountryResult()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefault();

            return country;
        }

        public CountryResult FindByName(string name)
        {
            var country = _countryRepository.Get(x => x.Name == name)
                .Select(x => new CountryResult()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefault();

            return country;
        }

        public async Task<int> CreateAsync(CountryModifyRequest request)
        {
            var country = new Country()
            {
                Code = request.Code,
                Name = request.Name
            };

            var id = await _countryRepository.AddWithInt32EntityAsync(country);
            return id;
        }

        public async Task<bool> UpdateAsync(CountryModifyRequest request)
        {
            await _countryRepository.Get(x => x.Id == request.Id)
                .Set(x => x.Code, request.Code)
                .Set(x => x.Name, request.Name)
                .UpdateAsync();

            return true;
        }
    }
}
