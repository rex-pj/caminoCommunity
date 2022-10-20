using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Identifiers;
using Camino.Application.Contracts.AppServices.Identifiers.Dtos;
using Camino.Core.Contracts.Repositories.Identities;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Identifiers;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Camino.Application.AppServices.Identifiers
{
    public class CountryAppService : ICountryAppService, IScopedDependency
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IEntityRepository<Country> _countryEntityRepository;

        public CountryAppService(ICountryRepository countryRepository, IEntityRepository<Country> countryEntityRepository)
        {
            _countryRepository = countryRepository;
            _countryEntityRepository = countryEntityRepository;
        }

        public List<CountryResult> Get()
        {
            var countries = _countryRepository.Get();
            return countries.Select(x => new CountryResult()
            {
                Code = x.Code,
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public async Task<BasePageList<CountryResult>> GetAsync(CountryFilter filter)
        {
            var keyword = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var query = _countryEntityRepository.Get();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.ToLower().Contains(keyword) || x.Name.ToLower().Contains(keyword));
            }

            var filteredNumber = query.Select(x => x.Id).Count();

            var countries = await query.OrderBy(x => x.Code).Skip(filter.PageSize * (filter.Page - 1))
                            .Take(filter.PageSize)
                            .Select(x => new CountryResult()
                            {
                                Code = x.Code,
                                Id = x.Id,
                                Name = x.Name
                            })
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
            var data = _countryEntityRepository.Get(x => string.IsNullOrEmpty(keyword) || x.Name.ToLower().Contains(keyword)
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
            var existing = _countryRepository.Find(id);
            if (existing == null)
            {
                return null;
            }

            return new CountryResult
            {
                Id = existing.Id,
                Code = existing.Code,
                Name = existing.Name
            };
        }

        public CountryResult FindByName(string name)
        {
            var existing = _countryRepository.FindByName(name);
            if (existing == null)
            {
                return null;
            }

            return new CountryResult
            {
                Id = existing.Id,
                Code = existing.Code,
                Name = existing.Name
            };
        }

        public async Task<int> CreateAsync(CountryModifyRequest request)
        {
            var country = new Country()
            {
                Code = request.Code,
                Name = request.Name
            };
            return await _countryRepository.CreateAsync(country);
        }

        public async Task<bool> UpdateAsync(CountryModifyRequest request)
        {
            var existing = _countryRepository.Find(request.Id);
            existing.Code = request.Code;
            existing.Name = request.Name;
            return await _countryRepository.UpdateAsync(existing);
        }
    }
}
