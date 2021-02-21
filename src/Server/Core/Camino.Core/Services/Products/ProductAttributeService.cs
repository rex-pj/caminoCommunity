using Camino.Core.Contracts.Services.Products;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Products;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.Requests.Products;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Shared.General;

namespace Camino.Services.Products
{
    public class ProductAttributeService : IProductAttributeService
    {
        private readonly IProductAttributeRepository _productAttributeRepository;

        public ProductAttributeService(IProductAttributeRepository productAttributeRepository)
        {
            _productAttributeRepository = productAttributeRepository;
        }

        public ProductAttributeResult Find(long id)
        {
            var productAttribute = _productAttributeRepository.Find(id);
            return productAttribute;
        }

        public ProductAttributeResult FindByName(string name)
        {
            var productAttribute = _productAttributeRepository.FindByName(name);
            return productAttribute;
        }

        public async Task<BasePageList<ProductAttributeResult>> GetAsync(ProductAttributeFilter filter)
        {
            var result = await _productAttributeRepository.GetAsync(filter);
            return result;
        }

        public async Task<IList<ProductAttributeResult>> SearchAsync(string search = "", int page = 1, int pageSize = 10)
        {
            return await _productAttributeRepository.SearchAsync(search, page, pageSize);
        }

        public async Task<int> CreateAsync(ProductAttributeModifyRequest request)
        {
            return await _productAttributeRepository.CreateAsync(request);
        }

        public async Task<bool> UpdateAsync(ProductAttributeModifyRequest request)
        {
            return await _productAttributeRepository.UpdateAsync(request); ;
        }

        public IList<SelectOption> GetAttributeControlTypes(ProductAttributeControlTypeFilter filter)
        {
            return _productAttributeRepository.GetAttributeControlTypes(filter);
        }
    }
}