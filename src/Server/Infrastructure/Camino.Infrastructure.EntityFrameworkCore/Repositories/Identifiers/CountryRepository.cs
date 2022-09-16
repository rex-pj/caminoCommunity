using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Identities;
using Camino.Core.Domains.Identifiers;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Camino.Core.Domains.Users;
using static System.Formats.Asn1.AsnWriter;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Identifiers
{
    public class CountryRepository : ICountryRepository, IScopedDependency
    {
        private readonly IEntityRepository<Country> _countryRepository;
        private readonly IAppDbContext _dbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CountryRepository(IEntityRepository<Country> countryRepository, IAppDbContext dbContext, 
            IServiceScopeFactory serviceScopeFactory)
        {
            _countryRepository = countryRepository;
            _dbContext = dbContext;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public List<Country> Get()
        {
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var countryRepository = scope.ServiceProvider.GetRequiredService<IEntityRepository<Country>>();
                return countryRepository.Get().ToList();
            }
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
