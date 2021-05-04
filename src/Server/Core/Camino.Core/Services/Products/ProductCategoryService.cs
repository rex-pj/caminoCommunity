using Camino.Shared.Requests.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Camino.Core.Contracts.Services.Products;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Products;
using Camino.Core.Domain.Products;
using Camino.Shared.Requests.Products;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Core.Contracts.Repositories.Users;
using System.Linq;

namespace Camino.Services.Products
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IUserRepository _userRepository;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IUserRepository userRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _userRepository = userRepository;
        }

        public async Task<ProductCategoryResult> FindAsync(int id)
        {
            var category = await _productCategoryRepository.FindAsync(id);
            if (category == null)
            {
                return category;
            }

            var createdByUser = await _userRepository.FindByIdAsync(category.CreatedById);
            var updatedByUser = await _userRepository.FindByIdAsync(category.UpdatedById);
            category.CreatedBy = createdByUser.DisplayName;
            category.UpdatedBy = updatedByUser.DisplayName;

            return category;
        }

        public ProductCategoryResult FindByName(string name)
        {
            return _productCategoryRepository.FindByName(name);
        }

        public async Task<BasePageList<ProductCategoryResult>> GetAsync(ProductCategoryFilter filter)
        {
            var productCategoriesPageList = await _productCategoryRepository.GetAsync(filter);

            var createdByIds = productCategoriesPageList.Collections.Select(x => x.CreatedById).ToArray();
            var updatedByIds = productCategoriesPageList.Collections.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = await _userRepository.GetNameByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetNameByIdsAsync(updatedByIds);

            foreach (var category in productCategoriesPageList.Collections)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.UpdatedBy = updatedBy.DisplayName;
            }

            return productCategoriesPageList;
        }

        public List<ProductCategoryResult> Get(Expression<Func<ProductCategory, bool>> filter)
        {
            return _productCategoryRepository.Get(filter);
        }

        public async Task<IList<ProductCategoryResult>> SearchParentsAsync(int[] currentIds, string search = "", int page = 1, int pageSize = 10)
        {
            return await _productCategoryRepository.SearchParentsAsync(currentIds, search, page, pageSize);
        }

        public async Task<IList<ProductCategoryResult>> SearchAsync(int[] currentIds, string search = "", int page = 1, int pageSize = 10)
        {
            return await _productCategoryRepository.SearchAsync(currentIds, search, page, pageSize);
        }

        public async Task<int> CreateAsync(ProductCategoryRequest request)
        {
            return await _productCategoryRepository.CreateAsync(request);
        }

        public async Task<bool> UpdateAsync(ProductCategoryRequest request)
        {
            return await _productCategoryRepository.UpdateAsync(request);
        }
    }
}
