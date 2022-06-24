namespace Camino.Core.Domains.Products.DomainServices
{
    public interface IProductCategoryDomainService
    {
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateProductCategoryRelationsAsync(long productId, IList<long> categoryIds, bool needSaveChanges = false);
    }
}
