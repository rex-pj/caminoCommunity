using Camino.Application.Contracts.AppServices.Farms.Dtos;

namespace Camino.Application.Contracts.AppServices.Farms
{
    public interface IFarmAppService
    {
        Task<bool> ActivateAsync(FarmModifyRequest request);
        Task<long> CreateAsync(FarmModifyRequest request);
        Task<bool> DeactivateAsync(FarmModifyRequest request);
        Task<bool> DeleteAsync(long id);
        Task<FarmResult> FindAsync(IdRequestFilter<long> filter);
        Task<FarmResult> FindByNameAsync(string name);
        Task<FarmResult> FindDetailAsync(IdRequestFilter<long> filter);
        Task<BasePageList<FarmResult>> GetAsync(FarmFilter filter);
        Task<IList<FarmResult>> GetByTypeAsync(IdRequestFilter<long> filter);
        Task<BasePageList<FarmPictureResult>> GetPicturesAsync(FarmPictureFilter filter);
        Task PopulateDetailsAsync(IList<FarmResult> farms, FarmFilter filter);
        IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "");
        Task<IList<FarmResult>> SelectAsync(SelectFilter filter, int page, int pageSize);
        Task<bool> SoftDeleteAsync(FarmModifyRequest request);
        Task<bool> UpdateAsync(FarmModifyRequest request);
    }
}
