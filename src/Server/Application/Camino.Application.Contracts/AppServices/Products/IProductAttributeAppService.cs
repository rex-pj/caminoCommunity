using Camino.Application.Contracts.AppServices.Products.Dtos;

namespace Camino.Application.Contracts.AppServices.Products
{
    public interface IProductAttributeAppService
    {
        Task<bool> ActiveAsync(ProductAttributeModifyRequest request);
        Task<int> CreateAsync(ProductAttributeModifyRequest request);
        Task<bool> DeactivateAsync(ProductAttributeModifyRequest request);
        Task<ProductAttributeResult> FindAsync(IdRequestFilter<int> filter);
        Task<ProductAttributeResult> FindByNameAsync(string name);
        Task<BasePageList<ProductAttributeResult>> GetAsync(ProductAttributeFilter filter);
        IList<SelectOption> GetAttributeControlTypes(ProductAttributeControlTypeFilter filter);
        Task<ProductAttributeRelationResult> GetAttributeRelationByIdAsync(long id);
        Task<IList<ProductAttributeRelationResult>> GetAttributeRelationsByProductIdAsync(long productId);
        Task<ProductAttributeRelationValueResult> GetAttributeRelationValueByIdAsync(long id);
        Task PopulateModifiersAsync(IList<ProductAttributeResult> productAttributes);
        Task<IList<ProductAttributeResult>> SearchAsync(ProductAttributeFilter filter);
        IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "");
        Task<bool> UpdateAsync(ProductAttributeModifyRequest request);
    }
}