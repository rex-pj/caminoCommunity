using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Identities;
using Camino.Core.Domains.Identifiers;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;

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

        public List<Country> Get()
        {
            return _countryRepository.Get().ToList();
        }

        public Country Find(int id)
        {
            var country = _countryRepository.Get(x => x.Id == id).FirstOrDefault();
            return country;
        }

        public Country FindByName(string name)
        {
            var country = _countryRepository.Get(x => x.Name == name).FirstOrDefault();
            return country;
        }

        public async Task<int> CreateAsync(Country country)
        {
            await _countryRepository.InsertAsync(country);
            await _dbContext.SaveChangesAsync();
            return country.Id;
        }

        public async Task<bool> UpdateAsync(Country request)
        {
            var existing = await _countryRepository.FindAsync(x => x.Id == request.Id);
            existing.Code = request.Code;
            existing.Name = request.Name;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
