using Camino.Shared.General;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Requests.Products;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Services.Products
{
    public interface IProductAttributeService
    {
        Task<int> CreateAsync(ProductAttributeModifyRequest request);
        ProductAttributeResult Find(long id);
        ProductAttributeResult FindByName(string name);
        Task<BasePageList<ProductAttributeResult>> GetAsync(ProductAttributeFilter filter);
        Task<IList<ProductAttributeResult>> SearchAsync(ProductAttributeFilter filter);
        Task<bool> UpdateAsync(ProductAttributeModifyRequest request);
        IList<SelectOption> GetAttributeControlTypes(ProductAttributeControlTypeFilter filter);
    }
}