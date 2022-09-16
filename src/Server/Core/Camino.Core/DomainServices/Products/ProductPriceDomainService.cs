using Camino.Core.DependencyInjection;
using Camino.Core.Domains;
using Camino.Core.Domains.Products;
using Camino.Core.Domains.Products.DomainServices;
using System;
using System.Threading.Tasks;

namespace Camino.Core.DomainServices.Products
{
    public class ProductPriceDomainService : IProductPriceDomainService, IScopedDependency
    {
        private readonly IEntityRepository<ProductPrice> _productPriceRepository;
        private readonly IDbContext _dbContext;

        public ProductPriceDomainService(IEntityRepository<ProductPrice> productPriceRepository,
            IDbContext dbContext)
        {
            _dbContext = dbContext;
            _productPriceRepository = productPriceRepository;
        }

        public async Task<bool> UpdateProductPriceAsync(long productId, decimal price, bool needSaveChanges = false)
        {
            var modifiedDate = DateTime.UtcNow;
            var existingPrices = await _productPriceRepository
                .GetAsync(x => x.ProductId == productId && x.IsCurrent && x.Price != price);
            foreach (var existingPrice in existingPrices)
            {
                existingPrice.IsCurrent = false;
            }

            await _productPriceRepository.InsertAsync(new ProductPrice()
            {
                PricedDate = modifiedDate,
                ProductId = productId,
                Price = price,
                IsCurrent = true
            });

            if (needSaveChanges)
            {
                return (await _dbContext.SaveChangesAsync()) > 0;
            }

            return false;
        }
    }
}
