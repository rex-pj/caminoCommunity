using Camino.Core.Contracts.Repositories.Products;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains;
using Camino.Core.Domains.Products;
using Camino.Core.Domains.Products.DomainServices;
using Camino.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Core.DomainServices.Products
{
    public class ProductCategoryDomainService : IProductCategoryDomainService, IScopedDependency
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IEntityRepository<ProductCategoryRelation> _productCategoryRelationRepository;
        private readonly IDbContext _dbContext;

        public ProductCategoryDomainService(IEntityRepository<ProductCategoryRelation> productCategoryRelationRepository,
            IProductCategoryRepository productCategoryRepository,
            IDbContext dbContext)
        {
            _dbContext = dbContext;
            _productCategoryRelationRepository = productCategoryRelationRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<bool> UpdateProductCategoryRelationsAsync(long productId, IList<long> categoryIds, bool needSaveChanges = false)
        {
            var modifiedDate = DateTime.UtcNow;
            // Update Category
            await _productCategoryRelationRepository
                        .DeleteAsync(x => x.ProductId == productId && !categoryIds.Contains(x.ProductCategoryId));

            var linkedCategoryIds = _productCategoryRelationRepository
                .Get(x => x.ProductId == productId && categoryIds.Contains(x.ProductCategoryId))
                .Select(x => x.ProductCategoryId)
                .ToList();

            var unlinkedCategoryIds = categoryIds.Where(x => !linkedCategoryIds.Contains(x));
            if (unlinkedCategoryIds != null && unlinkedCategoryIds.Any())
            {
                foreach (var categoryId in unlinkedCategoryIds)
                {
                    _productCategoryRelationRepository.Insert(new ProductCategoryRelation
                    {
                        ProductCategoryId = categoryId,
                        ProductId = productId
                    });
                }

                if (needSaveChanges)
                {
                    return (await _dbContext.SaveChangesAsync()) > 0;
                }
            }

            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var hasProducts = await _productCategoryRepository.HasProductsAsync(id);
            if (hasProducts)
            {
                throw new CaminoApplicationException($"Some Products belong to this category need to be deleted or move to another category");
            }

            return await _productCategoryRepository.DeleteAsync(id);
        }
    }
}
