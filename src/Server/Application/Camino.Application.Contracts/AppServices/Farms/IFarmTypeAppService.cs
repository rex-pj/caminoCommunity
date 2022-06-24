using Camino.Application.Contracts.AppServices.Farms.Dtos;

namespace Camino.Application.Contracts.AppServices.Farms
{
    public interface IFarmTypeAppService
    {
        Task<bool> ActiveAsync(FarmTypeModifyRequest farmType);
        Task<long> CreateAsync(FarmTypeModifyRequest farmType);
        Task<bool> DeactivateAsync(FarmTypeModifyRequest farmType);
        Task<bool> DeleteAsync(int id);
        Task<FarmTypeResult> FindAsync(long id);
        Task<FarmTypeResult> FindByNameAsync(string name);
        Task<BasePageList<FarmTypeResult>> GetAsync(FarmTypeFilter filter);
        Task<IList<FarmTypeResult>> SearchAsync(BaseFilter filter);
        IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "");
        Task<bool> UpdateAsync(FarmTypeModifyRequest farmType);
    }
}
