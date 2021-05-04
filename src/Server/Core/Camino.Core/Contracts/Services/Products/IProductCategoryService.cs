using Camino.Shared.Requests.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Products;
using Camino.Core.Domain.Products;
using Camino.Shared.Requests.Products;

namespace Camino.Core.Contracts.Services.Products
{
    public interface IProductCategoryService
    {
        Task<ProductCategoryResult> FindAsync(int id);
        Task<BasePageList<ProductCategoryResult>> GetAsync(ProductCategoryFilter filter);
        Task<IList<ProductCategoryResult>> SearchAsync(int[] currentIds, string search = "", int page = 1, int pageSize = 10);
        Task<IList<ProductCategoryResult>> SearchParentsAsync(int[] currentIds, string search = "", int page = 1, int pageSize = 10);
        List<ProductCategoryResult> Get(Expression<Func<ProductCategory, bool>> filter);
        Task<int> CreateAsync(ProductCategoryRequest request);
        Task<bool> UpdateAsync(ProductCategoryRequest request);
        ProductCategoryResult FindByName(string name);
    }
}
