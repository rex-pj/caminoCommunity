using Camino.Core.Domains.Identifiers;

namespace Camino.Core.Contracts.Repositories.Identities
{
    public interface ICountryRepository
    {
        List<Country> Get();
        Country Find(int id);
        Country FindByName(string name);
        Task<int> CreateAsync(Country country);
        Task<bool> UpdateAsync(Country country);
    }
}
