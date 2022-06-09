using Camino.Core.Contracts.Repositories.Products;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains;
using Camino.Core.Domains.Products;
using Camino.Core.Domains.Products.DomainServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Core.DomainServices.Products
{
    public class ProductAttributeDomainService : IProductAttributeDomainService, IScopedDependency
    {
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IEntityRepository<ProductAttributeRelation> _productAttributeRelationRepository;
        private readonly IEntityRepository<ProductAttributeRelationValue> _productAttributeRelationValueRepository;
        private readonly IDbContext _dbContext;

        public ProductAttributeDomainService(IDbContext dbContext,
            IProductAttributeRepository productAttributeRepository,
            IEntityRepository<ProductAttributeRelation> productAttributeRelationRepository,
            IEntityRepository<ProductAttributeRelationValue> productAttributeRelationValueRepository)
        {
            _dbContext = dbContext;
            _productAttributeRepository = productAttributeRepository;
            _productAttributeRelationRepository = productAttributeRelationRepository;
            _productAttributeRelationValueRepository = productAttributeRelationValueRepository;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await DeleteRelationByAttributeIdAsync(id);
            return await _productAttributeRepository.DeleteAsync(id);
        }

        public async Task<bool> IsAttributeRelationExistAsync(long relationId)
        {
            var isExist = (await _productAttributeRelationRepository.GetAsync(x => x.Id == relationId, x => x.Id)).Any();
            return isExist;
        }

        public async Task<long> CreateRelationAsync(ProductAttributeRelation attributeRelation, bool needSaveChanges = false)
        {
            var attributeRelationId = await _productAttributeRepository.CreateRelationAsync(new ProductAttributeRelation
            {
                AttributeControlTypeId = attributeRelation.AttributeControlTypeId,
                DisplayOrder = attributeRelation.DisplayOrder,
                IsRequired = attributeRelation.IsRequired,
                ProductAttributeId = attributeRelation.ProductAttributeId,
                ProductId = attributeRelation.ProductId,
                TextPrompt = attributeRelation.TextPrompt
            });

            if (attributeRelation.ProductAttributeRelationValues.Any())
            {
                foreach (var attributeValue in attributeRelation.ProductAttributeRelationValues)
                {
                    await _productAttributeRepository.CreateRelationValueAsync(attributeRelationId, attributeValue, needSaveChanges);
                }
            }

            return attributeRelationId;
        }

        public async Task<bool> UpdateRelationAsync(long relationId, ProductAttributeRelation attributeRelation, bool needSaveChanges = false)
        {
            if (!attributeRelation.ProductAttributeRelationValues.Any())
            {
                /// Delete product attribute and all attribute values
                await _productAttributeRelationValueRepository.DeleteAsync(x => x.ProductAttributeRelationId == relationId);
                await _productAttributeRelationRepository.DeleteAsync(x => x.Id == relationId);
            }

            var existingRelation = await _productAttributeRelationRepository.FindAsync(x => x.Id == relationId);
            existingRelation.IsRequired = attributeRelation.IsRequired;
            existingRelation.ProductAttributeId = attributeRelation.ProductAttributeId;
            existingRelation.TextPrompt = attributeRelation.TextPrompt;
            existingRelation.DisplayOrder = attributeRelation.DisplayOrder;
            existingRelation.AttributeControlTypeId = attributeRelation.AttributeControlTypeId;

            var attributeRelationValueIds = attributeRelation.ProductAttributeRelationValues.Where(x => x.Id != 0).Select(x => x.Id);
            await _productAttributeRelationValueRepository
                .DeleteAsync(x => x.ProductAttributeRelationId == relationId && !attributeRelationValueIds.Contains(x.Id));

            foreach (var attributeValue in attributeRelation.ProductAttributeRelationValues)
            {
                var isAttributeValueExist = attributeValue.Id != 0 && _productAttributeRelationValueRepository.Get(x => x.Id == attributeValue.Id).Any();
                if (!isAttributeValueExist)
                {
                    await _productAttributeRepository.CreateRelationValueAsync(relationId, attributeValue, needSaveChanges);
                }
                else
                {
                    var existingRelationValue = await _productAttributeRelationValueRepository.FindAsync(x => x.Id == attributeValue.Id);
                    existingRelationValue.PriceAdjustment = attributeValue.PriceAdjustment;
                    existingRelationValue.PricePercentageAdjustment = attributeValue.PricePercentageAdjustment;
                    existingRelationValue.Name = attributeValue.Name;
                    existingRelationValue.Quantity = attributeValue.Quantity;
                    existingRelationValue.DisplayOrder = attributeValue.DisplayOrder;
                    await _productAttributeRepository.UpdateRelationValueAsync(existingRelationValue, needSaveChanges);
                }
            }

            if (needSaveChanges)
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            
            return false;
        }

        public async Task<int> DeleteRelationNotInIdsAsync(long productId, IEnumerable<long> relationIds)
        {
            var deletedRelations = await _productAttributeRelationRepository.GetAsync(x => x.ProductId == productId && !relationIds.Contains(x.Id));
            var deletedRelationIds = deletedRelations.Select(x => x.Id);
            if (!deletedRelationIds.Any())
            {
                return 0;
            }

            await _productAttributeRelationValueRepository.DeleteAsync(x => deletedRelationIds.Contains(x.ProductAttributeRelationId));
            var deleted = await _productAttributeRelationRepository.DeleteAsync(deletedRelations);
            await _dbContext.SaveChangesAsync();
            return deleted;
        }

        public async Task DeleteRelationByProductIdAsync(long productId)
        {
            var productAttributeRelations = await _productAttributeRelationRepository.GetAsync(x => x.ProductId == productId);
            var productAttributeRelationIds = productAttributeRelations.Select(x => x.Id).ToList();
            await _productAttributeRelationValueRepository.DeleteAsync(x => productAttributeRelationIds.Contains(x.ProductAttributeRelationId));
            await _productAttributeRelationRepository.DeleteAsync(productAttributeRelations);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteRelationByAttributeIdAsync(int attributeId)
        {
            var productAttributeRelations = await _productAttributeRelationRepository.GetAsync(x => x.ProductAttributeId == attributeId);
            var productAttributeRelationIds = productAttributeRelations.Select(x => x.Id).ToList();
            await _productAttributeRelationValueRepository.DeleteAsync(x => productAttributeRelationIds.Contains(x.ProductAttributeRelationId));
            var deletedRecords = await _productAttributeRelationRepository.DeleteAsync(productAttributeRelations);

            await _dbContext.SaveChangesAsync();
            return deletedRecords > 0;
        }
    }
}
