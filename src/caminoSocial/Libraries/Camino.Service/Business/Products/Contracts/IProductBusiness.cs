﻿using Camino.Service.Projections.Filters;
using System.Threading.Tasks;
using Camino.Service.Projections.PageList;
using Camino.Service.Projections.Product;

namespace Camino.Service.Business.Products.Contracts
{
    public interface IProductBusiness
    {
        Task<long> CreateAsync(ProductProjection product);
        ProductProjection Find(long id);
        Task<ProductProjection> FindDetailAsync(long id);
        ProductProjection FindByName(string name);
        Task<ProductProjection> UpdateAsync(ProductProjection article);
        Task<BasePageList<ProductProjection>> GetAsync(ProductFilter filter);
    }
}