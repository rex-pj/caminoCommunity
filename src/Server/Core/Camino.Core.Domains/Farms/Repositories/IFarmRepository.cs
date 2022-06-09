namespace Camino.Core.Domains.Farms.Repositories
{
    public interface IFarmRepository
    {
        Task<long> CreateAsync(Farm farm);
        Task<bool> DeleteAsync(long id);
        Task<Farm> FindAsync(long id);
        Task<Farm> FindByNameAsync(string name);
        Task<IList<Farm>> GetByTypeAsync(long farmType);
        Task<bool> UpdateAsync(Farm farm);
    }
}
