using Camino.Service.Projections.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Camino.DAL.Entities;
using Camino.Service.Projections.PageList;
using Camino.Service.Projections.Product;

namespace Camino.Service.Business.Products.Contracts
{
    public interface IProductCategoryBusiness
    {
        ProductCategoryProjection Find(long id);
        Task<BasePageList<ProductCategoryProjection>> GetAsync(ProductCategoryFilter filter);
        IList<ProductCategoryProjection> Search(string search = "", long? currentId = null, int page = 1, int pageSize = 10);
        IList<ProductCategoryProjection> SearchParents(string search = "", long? currentId = null, int page = 1, int pageSize = 10);
        List<ProductCategoryProjection> Get(Expression<Func<ProductCategory, bool>> filter);
        public int Add(ProductCategoryProjection category);
        ProductCategoryProjection Update(ProductCategoryProjection category);
        ProductCategoryProjection FindByName(string name);
    }
}
