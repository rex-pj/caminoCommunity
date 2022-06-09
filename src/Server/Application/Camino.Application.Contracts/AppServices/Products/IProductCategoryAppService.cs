using Camino.Application.Contracts.AppServices.Products.Dtos;

namespace Camino.Application.Contracts.AppServices.Products
{
    public interface IProductCategoryAppService
    {
        Task<bool> ActiveAsync(ProductCategoryRequest request);
        Task<int> CreateAsync(ProductCategoryRequest request);
        Task<bool> DeactivateAsync(ProductCategoryRequest request);
        Task<bool> DeleteAsync(int id);
        Task<ProductCategoryResult> FindAsync(int id);
        Task<ProductCategoryResult> FindByNameAsync(string name);
        Task<BasePageList<ProductCategoryResult>> GetAsync(ProductCategoryFilter filter);
        Task PopulateDetailsAsync(IList<ProductCategoryResult> productCategories);
        Task<IList<ProductCategoryResult>> SearchAsync(BaseFilter filter, int[] currentIds);
        Task<IList<ProductCategoryResult>> SearchParentsAsync(BaseFilter filter, int[] currentIds);
        IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "");
        Task<bool> UpdateAsync(ProductCategoryRequest request);
    }
}
