namespace Camino.Core.Domains.Products.DomainServices
{
    public interface IProductAttributeDomainService
    {
        Task<bool> DeleteAsync(int id);
        Task<bool> IsAttributeRelationExistAsync(long relationId);
        Task<long> CreateRelationAsync(ProductAttributeRelation attributeRelation, bool needSaveChanges = false);
        Task<bool> UpdateRelationAsync(long relationId, ProductAttributeRelation attributeRelation, bool needSaveChanges = false);
        Task DeleteRelationByProductIdAsync(long productId);
        Task<bool> DeleteRelationByAttributeIdAsync(int attributeId);
        Task<int> DeleteRelationNotInIdsAsync(long productId, IEnumerable<long> relationIds);
    }
}
