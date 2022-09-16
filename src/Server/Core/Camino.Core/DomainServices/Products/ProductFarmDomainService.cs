using Camino.Core.DependencyInjection;
using Camino.Core.Domains;
using Camino.Core.Domains.Farms;
using Camino.Core.Domains.Products.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Core.DomainServices.Products
{
    public class ProductFarmDomainService : IProductFarmDomainService, IScopedDependency
    {
        private readonly IEntityRepository<FarmProduct> _farmProductRepository;
        private readonly IDbContext _dbContext;

        public ProductFarmDomainService(IEntityRepository<FarmProduct> farmProductRepository,
            IDbContext dbContext)
        {
            _dbContext = dbContext;
            _farmProductRepository = farmProductRepository;
        }

        public async Task<bool> UpdateProductFarmRelationsAsync(long productId, IList<long> farmIds,
            long modifiedById, bool needSaveChanges = false)
        {
            var modifiedDate = DateTime.UtcNow;
            await _farmProductRepository.DeleteAsync(x => x.ProductId == productId && !farmIds.Contains(x.FarmId));

            var linkedFarmIds = _farmProductRepository
                .Get(x => x.ProductId == productId && farmIds.Contains(x.FarmId))
                .Select(x => x.FarmId)
                .ToList();

            var unlinkedFarmIds = linkedFarmIds.Where(x => !linkedFarmIds.Contains(x));
            if (unlinkedFarmIds != null && unlinkedFarmIds.Any())
            {
                foreach (var farmId in unlinkedFarmIds)
                {
                    _farmProductRepository.Insert(new FarmProduct()
                    {
                        FarmId = farmId,
                        ProductId = productId,
                        IsLinked = true,
                        LinkedById = modifiedById,
                        LinkedDate = modifiedDate
                    });
                }

                if (needSaveChanges)
                {
                    return (await _dbContext.SaveChangesAsync()) > 0;
                }
            }

            return false;
        }
    }
}
