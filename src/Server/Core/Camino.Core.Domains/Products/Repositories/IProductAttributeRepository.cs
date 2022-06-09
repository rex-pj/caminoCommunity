using Camino.Core.Domains.Products;

namespace Camino.Core.Contracts.Repositories.Products
{
    public interface IProductAttributeRepository
    {
        Task<int> CreateAsync(ProductAttribute productAttribute);
        Task<long> CreateRelationAsync(ProductAttributeRelation attributeRelation, bool needSaveChanges = false);
        Task<long> CreateRelationValueAsync(long productAttributeRelationId, ProductAttributeRelationValue attributeValue, bool needSaveChanges = false);
        Task<bool> DeleteAsync(int id);
        Task<ProductAttribute> FindAsync(int id);
        Task<ProductAttribute> FindByNameAsync(string name);
        Task<bool> UpdateAsync(ProductAttribute productAttribute);
        Task<bool> UpdateRelationValueAsync(ProductAttributeRelationValue attributeValue, bool needSaveChanges = false);
    }
}