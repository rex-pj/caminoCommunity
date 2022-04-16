using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Identifiers;
using Camino.Core.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Core.Contracts.Repositories.Identities;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Requests.Identifiers;
using Camino.Core.Contracts.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Camino.Infrastructure.EntityFrameworkCore.Extensions;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Identifiers
{
    public class CountryRepository : ICountryRepository, IScopedDependency
    {
        private readonly IEntityRepository<Country> _countryRepository;
        private readonly IAppDbContext _dbContext;

        public CountryRepository(IEntityRepository<Country> countryRepository, IAppDbContext dbContext)
        {
            _countryRepository = countryRepository;
            _dbContext = dbContext;
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
            var keyword = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var query = _countryRepository.Table
                .Select(x => new CountryResult()
                {
                    Code = x.Code,
                    Id = x.Id,
                    Name = x.Name
                });

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.ToLower().Contains(keyword) || x.Name.ToLower().Contains(keyword));
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

        public IList<CountryResult> Search(BaseFilter filter)
        {
            var keyword = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var data = _countryRepository.Get(x => string.IsNullOrEmpty(keyword) || x.Name.ToLower().Contains(keyword)
                || x.Code.ToLower().Contains(keyword));

            if (filter.PageSize > 0)
            {
                data = data.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
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

            await _countryRepository.InsertAsync(country);
            await _dbContext.SaveChangesAsync();
            return country.Id;
        }

        public async Task<bool> UpdateAsync(CountryModifyRequest request)
        {
            await _countryRepository.Get(x => x.Id == request.Id)
                .SetEntry(x => x.Code, request.Code)
                .SetEntry(x => x.Name, request.Name)
                .UpdateAsync();

            return true;
        }
    }
}
