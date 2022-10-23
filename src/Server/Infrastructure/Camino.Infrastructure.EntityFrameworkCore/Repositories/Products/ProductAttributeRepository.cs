using Camino.Core.Contracts.Repositories.Products;
using System;
using System.Threading.Tasks;
using Camino.Core.Domains.Products;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Products
{
    public class ProductAttributeRepository : IProductAttributeRepository, IScopedDependency
    {
        private readonly IEntityRepository<ProductAttribute> _productAttributeRepository;
        private readonly IEntityRepository<ProductAttributeRelation> _productAttributeRelationRepository;
        private readonly IEntityRepository<ProductAttributeRelationValue> _productAttributeRelationValueRepository;
        private readonly IDbContext _dbContext;
        public ProductAttributeRepository(IEntityRepository<ProductAttribute> productAttributeRepository,
            IEntityRepository<ProductAttributeRelation> productAttributeRelationRepository,
            IEntityRepository<ProductAttributeRelationValue> productAttributeRelationValueRepository,
            IDbContext dbContext)
        {
            _productAttributeRepository = productAttributeRepository;
            _productAttributeRelationRepository = productAttributeRelationRepository;
            _productAttributeRelationValueRepository = productAttributeRelationValueRepository;
            _dbContext = dbContext;
        }

        #region Attributes
        public async Task<ProductAttribute> FindAsync(int id)
        {
            var productAttribute = await _productAttributeRepository.FindAsync(x => x.Id == id);
            return productAttribute;
        }

        public async Task<ProductAttribute> FindByNameAsync(string name)
        {
            var productAttribute = await _productAttributeRepository.FindAsync(x => x.Name == name);
            return productAttribute;
        }

        public async Task<int> CreateAsync(ProductAttribute productAttribute)
        {
            var modifiedDate = DateTime.UtcNow;
            productAttribute.CreatedDate = modifiedDate;
            productAttribute.UpdatedDate = modifiedDate;
            await _productAttributeRepository.InsertAsync(productAttribute);
            await _dbContext.SaveChangesAsync();
            return productAttribute.Id;
        }

        public async Task<bool> UpdateAsync(ProductAttribute productAttribute)
        {
            var modifiedDate = DateTime.UtcNow;
            productAttribute.UpdatedDate = modifiedDate;
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedNumbers = await _productAttributeRepository.DeleteAsync(x => x.Id == id);
            await _dbContext.SaveChangesAsync();
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
        #endregion

        #region Attribute Relations
        public async Task<long> CreateRelationAsync(ProductAttributeRelation attributeRelation, bool needSaveChanges = false)
        {
            await _productAttributeRelationRepository.InsertAsync(attributeRelation);
            await _dbContext.SaveChangesAsync();
            if (needSaveChanges)
            {
                await _dbContext.SaveChangesAsync();
                return attributeRelation.Id;
            }
            return -1;
        }
        #endregion

        #region Attribute relation values
        public async Task<long> CreateRelationValueAsync(long productAttributeRelationId,
            ProductAttributeRelationValue attributeValue, bool needSaveChanges = false)
        {
            attributeValue.ProductAttributeRelationId = productAttributeRelationId;
            await _productAttributeRelationValueRepository.InsertAsync(attributeValue);
            if (needSaveChanges)
            {
                await _dbContext.SaveChangesAsync();
                return attributeValue.Id;
            }
            return -1;
        }

        public async Task<bool> UpdateRelationValueAsync(ProductAttributeRelationValue attributeValue, bool needSaveChanges = false)
        {
            await _productAttributeRelationValueRepository.UpdateAsync(attributeValue);
            if (needSaveChanges)
            {
                return (await _dbContext.SaveChangesAsync()) > 0;
            }
            return false;
        }
        #endregion
    }
}