using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Products;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Domain.Products;
using Camino.Shared.Requests.Products;
using Camino.Shared.General;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using LinqToDB.Tools;
using Camino.Core.Contracts.DependencyInjection;
using Camino.Infrastructure.Linq2Db.Extensions;

namespace Camino.Infrastructure.Repositories.Products
{
    public class ProductAttributeRepository : IProductAttributeRepository, IScopedDependency
    {
        private readonly IEntityRepository<ProductAttribute> _productAttributeRepository;
        private readonly IEntityRepository<ProductAttributeRelation> _productAttributeRelationRepository;
        private readonly IEntityRepository<ProductAttributeRelationValue> _productAttributeRelationValueRepository;

        public ProductAttributeRepository(IEntityRepository<ProductAttribute> productAttributeRepository,
            IEntityRepository<ProductAttributeRelation> productAttributeRelationRepository,
            IEntityRepository<ProductAttributeRelationValue> productAttributeRelationValueRepository)
        {
            _productAttributeRepository = productAttributeRepository;
            _productAttributeRelationRepository = productAttributeRelationRepository;
            _productAttributeRelationValueRepository = productAttributeRelationValueRepository;
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

            var id = await _productAttributeRepository.AddAsync<int>(newProductAttribute);
            return id;
        }

        public async Task<bool> UpdateAsync(ProductAttributeModifyRequest category)
        {
            await _productAttributeRepository.Get(x => x.Id == category.Id)
                .SetEntry(x => x.Description, category.Description)
                .SetEntry(x => x.UpdatedById, category.UpdatedById)
                .SetEntry(x => x.Name, category.Name)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> DeactivateAsync(ProductAttributeModifyRequest request)
        {
            await _productAttributeRepository.Get(x => x.Id == request.Id)
                .SetEntry(x => x.StatusId, (int)ArticleCategoryStatus.Inactived)
                .SetEntry(x => x.UpdatedById, request.UpdatedById)
                .SetEntry(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> ActiveAsync(ProductAttributeModifyRequest request)
        {
            await _productAttributeRepository.Get(x => x.Id == request.Id)
                .SetEntry(x => x.StatusId, (int)ArticleCategoryStatus.Actived)
                .SetEntry(x => x.UpdatedById, request.UpdatedById)
                .SetEntry(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedNumbers = await _productAttributeRepository.DeleteAsync(x => x.Id == id);
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
            return deleted;
        }

        public async Task CreateAttributeRelationAsync(ProductAttributeRelationRequest request)
        {
            var productAttributeRelationId = await _productAttributeRelationRepository.AddAsync<int>(new ProductAttributeRelation
            {
                AttributeControlTypeId = request.ControlTypeId,
                DisplayOrder = request.DisplayOrder,
                IsRequired = request.IsRequired,
                ProductAttributeId = request.ProductAttributeId,
                ProductId = request.ProductId,
                TextPrompt = request.TextPrompt
            });

            if (request.AttributeRelationValues.Any())
            {
                foreach (var attributeValue in request.AttributeRelationValues)
                {
                    await CreateAttributeRelationValueAsync(productAttributeRelationId, attributeValue);
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

            var attributeRelationUpdated = await (_productAttributeRelationRepository
                .Get(x => x.Id == request.Id)
                .SetEntry(x => x.IsRequired, request.IsRequired)
                .SetEntry(x => x.ProductAttributeId, request.ProductAttributeId)
                .SetEntry(x => x.TextPrompt, request.TextPrompt)
                .SetEntry(x => x.DisplayOrder, request.DisplayOrder)
                .SetEntry(x => x.AttributeControlTypeId, request.ControlTypeId)
                .UpdateAsync());

            if (attributeRelationUpdated == 0)
            {
                return false;
            }

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
                    var attributeValueUpdated = await (_productAttributeRelationValueRepository
                        .Get(x => x.Id == attributeValue.Id)
                        .SetEntry(x => x.PriceAdjustment, attributeValue.PriceAdjustment)
                        .SetEntry(x => x.PricePercentageAdjustment, attributeValue.PricePercentageAdjustment)
                        .SetEntry(x => x.Name, attributeValue.Name)
                        .SetEntry(x => x.Quantity, attributeValue.Quantity)
                        .SetEntry(x => x.DisplayOrder, attributeValue.DisplayOrder)
                        .UpdateAsync());
                }
            }

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

            await _productAttributeRelationValueRepository.AddAsync<int>(new ProductAttributeRelationValue
            {
                Name = attributeValue.Name,
                ProductAttributeRelationId = productAttributeRelationId,
                PriceAdjustment = attributeValue.PriceAdjustment,
                PricePercentageAdjustment = attributeValue.PricePercentageAdjustment,
                Quantity = attributeValue.Quantity,
                DisplayOrder = attributeValue.DisplayOrder,
            });
        }

        public async Task DeleteAttributeRelationByProductIdAsync(long productId)
        {
            var productAttributeRelations = _productAttributeRelationRepository.Get(x => x.ProductId == productId);
            var productAttributeRelationIds = await productAttributeRelations.Select(x => x.Id).ToListAsync();
            await _productAttributeRelationValueRepository.DeleteAsync(x => productAttributeRelationIds.Contains(x.ProductAttributeRelationId));
            await _productAttributeRelationRepository.DeleteAsync(productAttributeRelations);
        }

        public async Task<bool> DeleteAttributeRelationByAttributeIdAsync(int attributeId)
        {
            var productAttributeRelations = _productAttributeRelationRepository.Get(x => x.ProductAttributeId == attributeId);
            var productAttributeRelationIds = await productAttributeRelations.Select(x => x.Id).ToListAsync();
            await _productAttributeRelationValueRepository.DeleteAsync(x => productAttributeRelationIds.Contains(x.ProductAttributeRelationId));
            return (await _productAttributeRelationRepository.DeleteAsync(productAttributeRelations)) > 0;
        }
    }
}