using Camino.Shared.Enums;
using Camino.Shared.General;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Requests.Products;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Products
{
    public interface IProductAttributeRepository
    {
        Task<int> CreateAsync(ProductAttributeModifyRequest productAttribute);
        ProductAttributeResult Find(long id);
        ProductAttributeResult FindByName(string name);
        Task<BasePageList<ProductAttributeResult>> GetAsync(ProductAttributeFilter filter);
        Task<IList<ProductAttributeResult>> SearchAsync(ProductAttributeFilter filter);
        Task<bool> UpdateAsync(ProductAttributeModifyRequest category);
        IList<SelectOption> GetAttributeControlTypes(ProductAttributeControlTypeFilter filter);
        Task CreateAttributeRelationAsync(ProductAttributeRelationRequest request);
        Task<bool> UpdateAttributeRelationAsync(ProductAttributeRelationRequest request);
        Task DeleteAttributeRelationByProductIdAsync(long productId);
        Task<IList<ProductAttributeRelationResult>> GetAttributeRelationsByProductIdAsync(long productId);
        Task<bool> IsAttributeRelationExistAsync(long id);
        Task<int> DeleteAttributeRelationNotInIdsAsync(long productId, IEnumerable<long> ids);
    }
}