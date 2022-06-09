namespace Camino.Core.Domains.Farms.Repositories
{
    public interface IFarmTypeRepository
    {
        Task<FarmType> FindAsync(long id);
        Task<FarmType> FindByNameAsync(string name);
        Task<int> CreateAsync(FarmType farmType);
        Task<bool> UpdateAsync(FarmType farmType);
        Task<bool> DeleteAsync(int id);
    }
}
