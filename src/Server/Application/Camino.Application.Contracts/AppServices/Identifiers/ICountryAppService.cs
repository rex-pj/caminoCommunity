using Camino.Application.Contracts.AppServices.Identifiers.Dtos;

namespace Camino.Application.Contracts.AppServices.Identifiers
{
    public interface ICountryAppService
    {
        List<CountryResult> Get();
        Task<BasePageList<CountryResult>> GetAsync(CountryFilter filter);
        IList<CountryResult> Search(BaseFilter filter);
        CountryResult Find(int id);
        CountryResult FindByName(string name);
        Task<int> CreateAsync(CountryModifyRequest request);
        Task<bool> UpdateAsync(CountryModifyRequest request);
    }
}
