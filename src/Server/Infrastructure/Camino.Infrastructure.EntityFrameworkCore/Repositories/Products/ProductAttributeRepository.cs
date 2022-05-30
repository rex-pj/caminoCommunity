using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Domain.Products;
using Camino.Shared.Requests.Products;
using Camino.Shared.General;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Camino.Core.Contracts.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Camino.Infrastructure.EntityFrameworkCore.Extensions;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Products
{
    public class ProductAttributeRepository : IProductAttributeRepository, IScopedDependency
    {
        private readonly IEntityRepository<ProductAttribute> _productAttributeRepository;
        private readonly IEntityRepository<ProductAttributeRelation> _productAttributeRelationRepository;
        private readonly IEntityRepository<ProductAttributeRelationValue> _productAttributeRelationValueRepository;
        private readonly IAppDbContext _dbContext;
        public ProductAttributeRepository(IEntityRepository<ProductAttribute> productAttributeRepository,
            IEntityRepository<ProductAttributeRelation> productAttributeRelationRepository,
            IEntityRepository<ProductAttributeRelationValue> productAttributeRelationValueRepository,
            IAppDbContext dbContext)
        {
            _productAttributeRepository = productAttributeRepository;
            _productAttributeRelationRepository = productAttributeRelationRepository;
            _productAttributeRelationValueRepository = productAttributeRelationValueRepository;
            _dbContext = dbContext;
        }

        public async Task<ProductAttributeResult> FindAsync(IdRequestFilter<int> filter)
        {
            var inactivedStatus = ProductAttributeStatus.Inactived.GetCode();
            var productAttribute = await _productAttributeRepository.Get(x => x.Id == filter.Id && (filter.CanGetInactived || x.StatusId != inactivedStatus))
                .Select(x => new ProductAttributeResult
                {
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Name,
                    StatusId = x.StatusId
                }).FirstOrDefaultAsync();

            return productAttribute;
        }

        public async Task<ProductAttributeResult> FindByNameAsync(string name)
        {
            var productAttribute = await _productAttributeRepository.Get(x => x.Name == name)
                .Select(x => new ProductAttributeResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    StatusId = x.StatusId
                })
                .FirstOrDefaultAsync();

            return productAttribute;
        }

        public async Task<BasePageList<ProductAttributeResult>> GetAsync(ProductAttributeFilter filter)
        {
            var inactivedStatus = ProductAttributeStatus.Inactived.GetCode();
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var productAttributeQuery = _productAttributeRepository.Get(x => filter.CanGetInactived || x.StatusId != inactivedStatus);
            if (!string.IsNullOrEmpty(search))
            {
                productAttributeQuery = productAttributeQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            var query = productAttributeQuery.Select(x => new ProductAttributeResult
            {
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                StatusId = x.StatusId,
                CreatedById = x.CreatedById,
                UpdatedById = x.UpdatedById,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate
            });

            if (filter.StatusId.HasValue)
            {
                query = query.Where(x => x.StatusId == filter.StatusId);
            }

            var filteredNumber = query.Select(x => x.Id).Count();

            var productAttributes = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var result = new BasePageList<ProductAttributeResult>(productAttributes)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task<IList<ProductAttributeResult>> SearchAsync(ProductAttributeFilter filter)
        {
            string search = filter.Keyword;
            if (search == null)
            {
                search = string.Empty;
            }

            var inactivedStatus = ProductAttributeStatus.Inactived.GetCode();
            search = search.ToLower();
            var query = _productAttributeRepository.Get(x => filter.CanGetInactived || x.StatusId != inactivedStatus);
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search)
                        || x.Description.ToLower().Contains(search));
            }

            if (filter.ExcludedIds.Any())
            {
                query = query.Where(x => !filter.ExcludedIds.Contains(x.Id));
            }

            if (filter.Id.HasValue)
            {
                query = query.Where(x => x.Id != filter.Id);
            }

            if (filter.PageSize > 0)
            {
                query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            }

            var productAttributes = await query
                .Select(x => new ProductAttributeResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    StatusId = x.StatusId,
                    CreatedById = x.CreatedById,
                    UpdatedById = x.UpdatedById,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                }).ToListAsync();

            return productAttributes;
        }

        public async Task<int> CreateAsync(ProductAttributeModifyRequest productAttribute)
        {
            var newProductAttribute = new ProductAttribute
            {
                Description = productAttribute.Description,
                Name = productAttribute.Name,
                StatusId = ProductAttributeStatus.Actived.GetCode(),
                CreatedById = productAttribute.CreatedById,
                UpdatedById = productAttribute.UpdatedById,
                UpdatedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
            };

            await _productAttributeRepository.InsertAsync(newProductAttribute);
            await _dbContext.SaveChangesAsync();
            return newProductAttribute.Id;
        }

        public async Task<bool> UpdateAsync(ProductAttributeModifyRequest category)
        {
            var existing = await _productAttributeRepository.FindAsync(x => x.Id == category.Id);
            existing.Description = category.Description;
            existing.UpdatedById = category.UpdatedById;
            existing.Name = category.Name;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeactivateAsync(ProductAttributeModifyRequest request)
        {
            var existing = await _productAttributeRepository.FindAsync(x => x.Id == request.Id);
            existing.StatusId = (int)ProductAttributeStatus.Inactived;
            existing.UpdatedById = request.UpdatedById;
            existing.UpdatedDate = DateTimeOffset.UtcNow;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> ActiveAsync(ProductAttributeModifyRequest request)
        {
            var existing = await _productAttributeRepository.FindAsync(x => x.Id == request.Id);
            existing.StatusId = (int)ProductAttributeStatus.Actived;
            existing.UpdatedById = request.UpdatedById;
            existing.UpdatedDate = DateTimeOffset.UtcNow;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedNumbers = await _productAttributeRepository.DeleteAsync(x => x.Id == id);
            await _dbContext.SaveChangesAsync();
            return deletedNumbers > 0;
        }

        public IList<SelectOption> GetAttributeControlTypes(ProductAttributeControlTypeFilter filter)
        {
            var result = EnumUtil.ToSelectOptions<ProductAttributeControlType>().ToList();

            if (filter.ControlTypeId > 0)
            {
                result = result.Where(x => x.Id != filter.ControlTypeId.ToString()).ToList();
            }

            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
            return result;
        }

        public async Task<bool> IsAttributeRelationExistAsync(long id)
        {
            var isExist = await (_productAttributeRelationRepository
                .Get(x => x.Id == id))
                .AnyAsync();

            return isExist;
        }

        public async Task<int> DeleteAttributeRelationNotInIdsAsync(long productId, IEnumerable<long> ids)
        {
            var productAttributeRelations = _productAttributeRelationRepository
                .Get(x => x.ProductId == productId && !ids.Contains(x.Id));
            var productAttributeRelationIds = await productAttributeRelations.Select(x => x.Id).ToListAsync();
            if (!productAttributeRelationIds.Any())
            {
                return 0;
            }

            await _productAttributeRelationValueRepository.DeleteAsync(x => productAttributeRelationIds.Contains(x.ProductAttributeRelationId));
            var deleted = await _productAttributeRelationRepository.DeleteAsync(productAttributeRelations);
            await _dbContext.SaveChangesAsync();
            return deleted;
        }

        public async Task CreateAttributeRelationAsync(ProductAttributeRelationRequest request)
        {
            var newProductAttributeRelation = new ProductAttributeRelation
            {
                AttributeControlTypeId = request.ControlTypeId,
                DisplayOrder = request.DisplayOrder,
                IsRequired = request.IsRequired,
                ProductAttributeId = request.ProductAttributeId,
                ProductId = request.ProductId,
                TextPrompt = request.TextPrompt
            };
            await _productAttributeRelationRepository.InsertAsync(newProductAttributeRelation);
            await _dbContext.SaveChangesAsync();

            if (request.AttributeRelationValues.Any())
            {
                foreach (var attributeValue in request.AttributeRelationValues)
                {
                    await CreateAttributeRelationValueAsync(newProductAttributeRelation.Id, attributeValue);
                }
            }
        }

        public async Task<bool> UpdateAttributeRelationAsync(ProductAttributeRelationRequest request)
        {
            if (!request.AttributeRelationValues.Any())
            {
                /// Delete product attribute and all attribute values
                await _productAttributeRelationValueRepository.DeleteAsync(x => x.ProductAttributeRelationId == request.Id);
                await _productAttributeRelationRepository.DeleteAsync(x => x.Id == request.Id);
            }

            var existingRelation = await _productAttributeRelationRepository.FindAsync(x => x.Id == request.Id);
            existingRelation.IsRequired = request.IsRequired;
            existingRelation.ProductAttributeId = request.ProductAttributeId;
            existingRelation.TextPrompt = request.TextPrompt;
            existingRelation.DisplayOrder = request.DisplayOrder;
            existingRelation.AttributeControlTypeId = request.ControlTypeId;

            var attributeRelationValueIds = request.AttributeRelationValues.Where(x => x.Id != 0).Select(x => x.Id);
            await _productAttributeRelationValueRepository
                .DeleteAsync(x => x.ProductAttributeRelationId == request.Id && !attributeRelationValueIds.Contains(x.Id));

            foreach (var attributeValue in request.AttributeRelationValues)
            {
                var isAttributeValueExist = attributeValue.Id != 0 && await _productAttributeRelationValueRepository
                    .Get(x => x.Id == attributeValue.Id).AnyAsync();

                if (!isAttributeValueExist)
                {
                    await CreateAttributeRelationValueAsync(request.Id, attributeValue);
                }
                else
                {
                    var existingRelationValue = await _productAttributeRelationValueRepository
                        .FindAsync(x => x.Id == attributeValue.Id);
                    existingRelationValue.PriceAdjustment = attributeValue.PriceAdjustment;
                    existingRelationValue.PricePercentageAdjustment = attributeValue.PricePercentageAdjustment;
                    existingRelationValue.Name = attributeValue.Name;
                    existingRelationValue.Quantity = attributeValue.Quantity;
                    existingRelationValue.DisplayOrder = attributeValue.DisplayOrder;
                }
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IList<ProductAttributeRelationResult>> GetAttributeRelationsByProductIdAsync(long productId)
        {
            var productAttributes = await (from pattr in _productAttributeRelationRepository.Table
                                           join attr in _productAttributeRepository.Table
                                           on pattr.ProductAttributeId equals attr.Id

                                           join attrv in _productAttributeRelationValueRepository.Table
                                           on pattr.Id equals attrv.ProductAttributeRelationId into attributeRelationValues

                                           where pattr.ProductId == productId
                                           select new ProductAttributeRelationResult
                                           {
                                               AttributeControlTypeId = pattr.AttributeControlTypeId,
                                               DisplayOrder = pattr.DisplayOrder,
                                               Id = pattr.Id,
                                               IsRequired = pattr.IsRequired,
                                               AttributeId = pattr.ProductAttributeId,
                                               TextPrompt = pattr.TextPrompt,
                                               AttributeName = attr.Name,
                                               AttributeRelationValues = attributeRelationValues.Select(x => new ProductAttributeRelationValueResult
                                               {
                                                   DisplayOrder = x.DisplayOrder,
                                                   Id = x.Id,
                                                   Name = x.Name,
                                                   PriceAdjustment = x.PriceAdjustment,
                                                   PricePercentageAdjustment = x.PricePercentageAdjustment,
                                                   ProductAttributeRelationId = x.ProductAttributeRelationId,
                                                   Quantity = x.Quantity
                                               })
                                           }).ToListAsync();

            return productAttributes;
        }

        public async Task<ProductAttributeRelationResult> GetAttributeRelationByIdAsync(long id)
        {
            var productAttribute = await (from pattr in _productAttributeRelationRepository.Table
                                          join attr in _productAttributeRepository.Table
                                          on pattr.ProductAttributeId equals attr.Id
                                          where pattr.Id == id
                                          select new ProductAttributeRelationResult
                                          {
                                              AttributeControlTypeId = pattr.AttributeControlTypeId,
                                              DisplayOrder = pattr.DisplayOrder,
                                              Id = pattr.Id,
                                              IsRequired = pattr.IsRequired,
                                              AttributeId = pattr.ProductAttributeId,
                                              TextPrompt = pattr.TextPrompt,
                                              AttributeName = attr.Name
                                          }).FirstOrDefaultAsync();

            return productAttribute;
        }

        public async Task<ProductAttributeRelationValueResult> GetAttributeRelationValueByIdAsync(long id)
        {
            var productAttributeValue = await (from atv in _productAttributeRelationValueRepository.Table
                                               where atv.Id == id
                                               select new ProductAttributeRelationValueResult
                                               {
                                                   DisplayOrder = atv.DisplayOrder,
                                                   Id = atv.Id,
                                                   Name = atv.Name,
                                                   PriceAdjustment = atv.PriceAdjustment,
                                                   PricePercentageAdjustment = atv.PricePercentageAdjustment,
                                                   ProductAttributeRelationId = atv.ProductAttributeRelationId,
                                                   Quantity = atv.Quantity
                                               }).FirstOrDefaultAsync();

            return productAttributeValue;
        }

        public async Task CreateAttributeRelationValueAsync(long productAttributeRelationId, ProductAttributeRelationValueRequest attributeValue)
        {

            await _productAttributeRelationValueRepository.InsertAsync(new ProductAttributeRelationValue
            {
                Name = attributeValue.Name,
                ProductAttributeRelationId = productAttributeRelationId,
                PriceAdjustment = attributeValue.PriceAdjustment,
                PricePercentageAdjustment = attributeValue.PricePercentageAdjustment,
                Quantity = attributeValue.Quantity,
                DisplayOrder = attributeValue.DisplayOrder,
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAttributeRelationByProductIdAsync(long productId)
        {
            var productAttributeRelations = _productAttributeRelationRepository.Get(x => x.ProductId == productId);
            var productAttributeRelationIds = await productAttributeRelations.Select(x => x.Id).ToListAsync();
            await _productAttributeRelationValueRepository.DeleteAsync(x => productAttributeRelationIds.Contains(x.ProductAttributeRelationId));
            await _productAttributeRelationRepository.DeleteAsync(productAttributeRelations);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAttributeRelationByAttributeIdAsync(int attributeId)
        {
            var productAttributeRelations = _productAttributeRelationRepository.Get(x => x.ProductAttributeId == attributeId);
            var productAttributeRelationIds = await productAttributeRelations.Select(x => x.Id).ToListAsync();
            await _productAttributeRelationValueRepository.DeleteAsync(x => productAttributeRelationIds.Contains(x.ProductAttributeRelationId));
            var deletedRecords = await _productAttributeRelationRepository.DeleteAsync(productAttributeRelations);

            await _dbContext.SaveChangesAsync();
            return deletedRecords > 0;
        }
    }
}