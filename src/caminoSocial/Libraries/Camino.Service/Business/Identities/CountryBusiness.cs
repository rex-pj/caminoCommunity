using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Identity;
using Camino.Data.Contracts;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Identities.Contracts;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.PageList;

namespace Camino.Service.Business.Identities
{
    public class CountryBusiness : ICountryBusiness
    {
        private readonly IRepository<Country> _countryRepository;

        public CountryBusiness(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public List<CountryProjection> GetAll()
        {
            return _countryRepository.Get()
                .Select(x => new CountryProjection()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code
                })
                .ToList();
        }

        public async Task<BasePageList<CountryProjection>> GetAsync(CountryFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var query = _countryRepository.Table
                .Select(x => new CountryProjection()
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

            var result = new BasePageList<CountryProjection>(countries)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public IList<CountryProjection> Search(string query = "", int page = 1, int pageSize = 10)
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
                .Select(x => new CountryProjection()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code
                })
                .ToList();

            return countries;
        }

        public CountryProjection Find(int id)
        {
            var country = _countryRepository.Get(x => x.Id == id)
                .Select(x => new CountryProjection()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefault();

            return country;
        }

        public CountryProjection FindByName(string name)
        {
            var country = _countryRepository.Get(x => x.Name == name)
                .Select(x => new CountryProjection()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .FirstOrDefault();

            return country;
        }

        public int Create(CountryProjection countryRequest)
        {
            var country = new Country()
            {
                Code = countryRequest.Code,
                Name = countryRequest.Name
            };

            var id = _countryRepository.AddWithInt32Entity(country);
            return id;
        }

        public CountryProjection Update(CountryProjection countryRequest)
        {
            var exist = _countryRepository.FirstOrDefault(x => x.Id == countryRequest.Id);
            if (exist == null)
            {
                return null;
            }
            exist.Code = countryRequest.Code;
            exist.Name = countryRequest.Name;

            _countryRepository.Update(exist);
            return countryRequest;
        }
    }
}
