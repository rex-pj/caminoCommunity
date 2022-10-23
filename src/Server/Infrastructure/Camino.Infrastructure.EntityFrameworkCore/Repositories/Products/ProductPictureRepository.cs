using Camino.Core.Contracts.Repositories.Products;
using System.Threading.Tasks;
using Camino.Core.Domains.Products;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;
using Camino.Shared.Enums;
using Camino.Shared.Utils;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Products
{
    public class ProductPictureRepository : IProductPictureRepository, IScopedDependency
    {
        private readonly IEntityRepository<ProductPicture> _productPictureRepository;
        private readonly IDbContext _dbContext;

        public ProductPictureRepository(IEntityRepository<ProductPicture> productPictureRepository,
            IDbContext dbContext)
        {
            _productPictureRepository = productPictureRepository;
            _dbContext = dbContext;
        }

        public async Task<ProductPicture> GetByTypeAsync(long productId, ProductPictureTypes pictureType)
        {
            return await _productPictureRepository
                .FindAsync(x => x.ProductId == productId && x.PictureTypeId == pictureType.GetCode());
        }

        public async Task<long> CreateAsync(ProductPicture productPicture, bool needSaveChanges = false)
        {
            await _productPictureRepository.InsertAsync(productPicture);
            if (needSaveChanges)
            {
                await _dbContext.SaveChangesAsync();
                return productPicture.Id;
            }
            return -1;
        }

        public async Task UpdateAsync(ProductPicture productPicture, bool needSaveChanges = false)
        {
            await _productPictureRepository.UpdateAsync(productPicture);
            if (needSaveChanges)
            {
                return;
            }
        }
    }
}
