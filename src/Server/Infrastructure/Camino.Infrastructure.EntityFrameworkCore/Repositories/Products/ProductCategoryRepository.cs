using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Products
{
    public class ProductCategoryRepository : IProductCategoryRepository, IScopedDependency
    {
        private readonly IEntityRepository<Product> _productRepository;
        private readonly IEntityRepository<ProductCategory> _productCategoryRepository;
        private readonly IEntityRepository<ProductCategoryRelation> _productCategoryRelationRepository;
        private readonly IDbContext _dbContext;

        public ProductCategoryRepository(IEntityRepository<ProductCategory> productCategoryRepository,
            IEntityRepository<Product> productRepository,
            IEntityRepository<ProductCategoryRelation> productCategoryRelationRepository,
            IDbContext dbContext)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _productCategoryRelationRepository = productCategoryRelationRepository;
            _dbContext = dbContext;
        }

        public async Task<ProductCategory> FindAsync(long id)
        {
            var category = await _productCategoryRepository.Table.Include(x => x.ParentCategory)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            return category;
        }

        public async Task<ProductCategory> FindByNameAsync(string name)
        {
            var category = await _productCategoryRepository.Get(x => x.Name == name)
                .FirstOrDefaultAsync();
            return category;
        }

        public async Task<long> CreateAsync(ProductCategory category)
        {
            var modifiedDate = DateTime.UtcNow;
            category.CreatedDate = modifiedDate;
            category.UpdatedDate = modifiedDate;           

            await _productCategoryRepository.InsertAsync(category);
            await _dbContext.SaveChangesAsync();
            return category.Id;
        }

        public async Task<bool> UpdateAsync(ProductCategory category)
        {
            category.UpdatedDate = DateTime.UtcNow;
            await _productCategoryRepository.UpdateAsync(category);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> HasProductsAsync(long categoryId)
        {
            return await (from relation in _productCategoryRelationRepository.Get(x => x.ProductCategoryId == categoryId)
                          join product in _productRepository.Table
                          on relation.ProductId equals product.Id
                          select product.Id)
                .AnyAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var deletedNumbers = await _productCategoryRepository.DeleteAsync(x => x.Id == id);
            await _dbContext.SaveChangesAsync();
            return deletedNumbers > 0;
        }
    }
}
