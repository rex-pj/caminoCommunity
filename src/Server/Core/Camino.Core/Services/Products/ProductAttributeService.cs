using Camino.Core.Contracts.Services.Products;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Products;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.Requests.Products;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Shared.General;
using Camino.Core.Contracts.Repositories.Users;
using System.Linq;
using Camino.Core.Contracts.DependencyInjection;

namespace Camino.Services.Products
{
    public class ProductAttributeService : IProductAttributeService, IScopedDependency
    {
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IProductAttributeStatusRepository _productAttributeStatusRepository;
        private readonly IUserRepository _userRepository;

        public ProductAttributeService(IProductAttributeRepository productAttributeRepository,
            IProductAttributeStatusRepository productAttributeStatusRepository,
             IUserRepository userRepository)
        {
            _productAttributeRepository = productAttributeRepository;
            _productAttributeStatusRepository = productAttributeStatusRepository;
            _userRepository = userRepository;
        }

        #region get
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
            var attributesPageList = await _productAttributeRepository.GetAsync(filter);
            var createdByIds = attributesPageList.Collections.Select(x => x.CreatedById).ToArray();
            var updatedByIds = attributesPageList.Collections.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = await _userRepository.GetNameByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetNameByIdsAsync(updatedByIds);

            foreach (var category in attributesPageList.Collections)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.UpdatedBy = updatedBy.DisplayName;
            }
            return attributesPageList;
        }

        public async Task<IList<ProductAttributeResult>> SearchAsync(ProductAttributeFilter filter)
        {
            return await _productAttributeRepository.SearchAsync(filter);
        }

        public IList<SelectOption> GetAttributeControlTypes(ProductAttributeControlTypeFilter filter)
        {
            return _productAttributeRepository.GetAttributeControlTypes(filter);
        }
        #endregion

        #region CRUD
        public async Task<int> CreateAsync(ProductAttributeModifyRequest request)
        {
            return await _productAttributeRepository.CreateAsync(request);
        }

        public async Task<bool> UpdateAsync(ProductAttributeModifyRequest request)
        {
            return await _productAttributeRepository.UpdateAsync(request); ;
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
        #endregion

        #region category status
        public IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "")
        {
            return _productAttributeStatusRepository.Search(filter, search);
        }
        #endregion
    }
}