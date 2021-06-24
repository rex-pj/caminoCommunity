using Camino.Core.Contracts.Services.Products;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Products;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.Requests.Products;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Shared.General;
using Camino.Core.Exceptions;

namespace Camino.Services.Products
{
    public class ProductAttributeService : IProductAttributeService
    {
        private readonly IProductAttributeRepository _productAttributeRepository;

        public ProductAttributeService(IProductAttributeRepository productAttributeRepository)
        {
            _productAttributeRepository = productAttributeRepository;
        }

        public async Task<ProductAttributeResult> FindAsync(IdRequestFilter<int> filter)
        {
            var productAttribute = await _productAttributeRepository.FindAsync(filter);
            return productAttribute;
        }

        public async Task<ProductAttributeResult> FindByNameAsync(string name)
        {
            var productAttribute = await _productAttributeRepository.FindByNameAsync(name);
            return productAttribute;
        }

        public async Task<BasePageList<ProductAttributeResult>> GetAsync(ProductAttributeFilter filter)
        {
            var result = await _productAttributeRepository.GetAsync(filter);
            return result;
        }

        public async Task<IList<ProductAttributeResult>> SearchAsync(ProductAttributeFilter filter)
        {
            return await _productAttributeRepository.SearchAsync(filter);
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

        public async Task<bool> ActiveAsync(ProductAttributeModifyRequest request)
        {
            return await _productAttributeRepository.ActiveAsync(request);
        }

        public async Task<bool> DeactivateAsync(ProductAttributeModifyRequest request)
        {
            return await _productAttributeRepository.DeactivateAsync(request);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _productAttributeRepository.DeleteAttributeRelationByAttributeIdAsync(id);
            return await _productAttributeRepository.DeleteAsync(id);
        }
    }
}