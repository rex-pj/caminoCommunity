using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Core.Domains.Products;
using Camino.Core.Domains.Farms;
using Microsoft.EntityFrameworkCore;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Products
{
    public class ProductRepository : IProductRepository, IScopedDependency
    {
        private readonly IEntityRepository<Product> _productRepository;
        private readonly IEntityRepository<FarmProduct> _farmProductRepository;
        private readonly IEntityRepository<ProductPrice> _productPriceRepository;
        private readonly IEntityRepository<ProductCategoryRelation> _productCategoryRelationRepository;
        private readonly IDbContext _dbContext;

        public ProductRepository(IEntityRepository<Product> productRepository,
            IEntityRepository<ProductCategoryRelation> productCategoryRelationRepository,
            IEntityRepository<ProductPrice> productPriceRepository, IEntityRepository<FarmProduct> farmProductRepository,
            IDbContext dbContext)
        {
            _productRepository = productRepository;
            _productCategoryRelationRepository = productCategoryRelationRepository;
            _productPriceRepository = productPriceRepository;
            _farmProductRepository = farmProductRepository;
            _dbContext = dbContext;
        }

        public async Task<Product> FindAsync(long id)
        {
            var exist = await (from product in _productRepository
                               .Get(x => x.Id == id)
                               select new Product
                               {
                                   CreatedDate = product.CreatedDate,
                                   CreatedById = product.CreatedById,
                                   Id = product.Id,
                                   Name = product.Name,
                                   Description = product.Description,
                                   UpdatedById = product.UpdatedById,
                                   UpdatedDate = product.UpdatedDate,
                                   StatusId = product.StatusId
                               }).FirstOrDefaultAsync();

            return exist;
        }

        public async Task<Product> FindByNameAsync(string name)
        {
            var existing = await _productRepository.FindAsync(x => x.Name == name);
            return existing;
        }

        public async Task<long> CreateAsync(Product product)
        {
            var modifiedDate = DateTime.UtcNow;
            product.CreatedDate = modifiedDate;
            product.UpdatedDate = modifiedDate;
            await _productRepository.InsertAsync(product);
            await _dbContext.SaveChangesAsync();

            return product.Id;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var modifiedDate = DateTime.UtcNow;
            product.UpdatedDate = modifiedDate;
            await _productRepository.UpdateAsync(product);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await _farmProductRepository.DeleteAsync(x => x.ProductId == id);

            await _productPriceRepository.DeleteAsync(x => x.ProductId == id);

            await _productCategoryRelationRepository.DeleteAsync(x => x.ProductId == id);

            await _productRepository.DeleteAsync(x => x.Id == id);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
