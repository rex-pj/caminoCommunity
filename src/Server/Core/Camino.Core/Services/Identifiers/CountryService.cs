using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Identifiers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Core.Contracts.Services.Identities;
using Camino.Shared.Requests.Identifiers;
using Camino.Core.Contracts.Repositories.Identities;

namespace Camino.Services.Identifiers
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public List<CountryResult> Get()
        {
            return _countryRepository.Get();
        }

        public async Task<BasePageList<CountryResult>> GetAsync(CountryFilter filter)
        {
            return await _countryRepository.GetAsync(filter);
        }

        public IList<CountryResult> Search(BaseFilter filter)
        {
            return _countryRepository.Search(filter);
        }

        public CountryResult Find(int id)
        {
            return _countryRepository.Find(id);
        }

        public CountryResult FindByName(string name)
        {
            return _countryRepository.FindByName(name);
        }

        public async Task<int> CreateAsync(CountryModifyRequest request)
        {
            return await _countryRepository.CreateAsync(request);
        }

        public async Task<bool> UpdateAsync(CountryModifyRequest request)
        {
            return await _countryRepository.UpdateAsync(request);
        }
    }
}
