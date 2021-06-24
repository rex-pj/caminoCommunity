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

namespace Camino.Service.Repository.Products
{
    public class ProductAttributeRepository : IProductAttributeRepository
    {
        private readonly IRepository<ProductAttribute> _productAttributeRepository;
        private readonly IRepository<ProductAttributeRelation> _productAttributeRelationRepository;
        private readonly IRepository<ProductAttributeRelationValue> _productAttributeRelationValueRepository;

        public ProductAttributeRepository(IRepository<ProductAttribute> productAttributeRepository,
            IRepository<ProductAttributeRelation> productAttributeRelationRepository,
            IRepository<ProductAttributeRelationValue> productAttributeRelationValueRepository)
        {
            _productAttributeRepository = productAttributeRepository;
            _productAttributeRelationRepository = productAttributeRelationRepository;
            _productAttributeRelationValueRepository = productAttributeRelationValueRepository;
        }

        public async Task<ProductAttributeResult> FindAsync(IdRequestFilter<int> filter)
        {
            var productAttribute = await _productAttributeRepository.Get(x => x.Id == filter.Id)
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
            var search = filter.Search != null ? filter.Search.ToLower() : "";
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
            string search = filter.Search;
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
                query = query.Where(x => x.Id.NotIn(filter.ExcludedIds));
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

            var id = await _productAttributeRepository.AddWithInt32EntityAsync(newProductAttribute);
            return id;
        }

        public async Task<bool> UpdateAsync(ProductAttributeModifyRequest category)
        {
            await _productAttributeRepository.Get(x => x.Id == category.Id)
                .Set(x => x.Description, category.Description)
                .Set(x => x.UpdatedById, category.UpdatedById)
                .Set(x => x.Name, category.Name)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> DeactivateAsync(ProductAttributeModifyRequest request)
        {
            await _productAttributeRepository.Get(x => x.Id == request.Id)
                .Set(x => x.StatusId, (int)ArticleCategoryStatus.Inactived)
                .Set(x => x.UpdatedById, request.UpdatedById)
                .Set(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> ActiveAsync(ProductAttributeModifyRequest request)
        {
            await _productAttributeRepository.Get(x => x.Id == request.Id)
                .Set(x => x.StatusId, (int)ArticleCategoryStatus.Actived)
                .Set(x => x.UpdatedById, request.UpdatedById)
                .Set(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedNumbers = await _productAttributeRepository.Get(x => x.Id == id).DeleteAsync();
            return deletedNumbers > 0;
        }

        public IList<SelectOption> GetAttributeControlTypes(ProductAttributeControlTypeFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var result = new List<SelectOption>();
            if (filter.ControlTypeId > 0)
            {
                var selected = (ProductAttributeControlType)filter.ControlTypeId;
                result = EnumUtil.ToSelectOptions(selected).ToList();
            }
            else
            {
                result = EnumUtil.ToSelectOptions<ProductAttributeControlType>().ToList();
            }

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
                .Get(x => x.ProductId == productId && x.Id.NotIn(ids));
            var productAttributeRelationIds = await productAttributeRelations.Select(x => x.Id).ToListAsync();
            if (!productAttributeRelationIds.Any())
            {
                return 0;
            }

            await _productAttributeRelationValueRepository.Get(x => x.ProductAttributeRelationId.In(productAttributeRelationIds))
                .DeleteAsync();
            var deleted = await productAttributeRelations.DeleteAsync();
            return deleted;
        }

        public async Task CreateAttributeRelationAsync(ProductAttributeRelationRequest request)
        {
            var productAttributeRelationId = await _productAttributeRelationRepository.AddWithInt32EntityAsync(new ProductAttributeRelation
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
                await _productAttributeRelationRepository.Get(x => x.Id == request.Id).DeleteAsync();
                await _productAttributeRelationValueRepository.Get(x => x.ProductAttributeRelationId == request.Id).DeleteAsync();
            }

            var attributeRelationUpdated = await (_productAttributeRelationRepository
                .Get(x => x.Id == request.Id)
                .Set(x => x.IsRequired, request.IsRequired)
                .Set(x => x.ProductAttributeId, request.ProductAttributeId)
                .Set(x => x.TextPrompt, request.TextPrompt)
                .Set(x => x.DisplayOrder, request.DisplayOrder)
                .Set(x => x.AttributeControlTypeId, request.ControlTypeId)
                .UpdateAsync());

            if (attributeRelationUpdated == 0)
            {
                return false;
            }

            var attributeRelationValueIds = request.AttributeRelationValues.Where(x => x.Id != 0).Select(x => x.Id);
            await _productAttributeRelationValueRepository
                .Get(x => x.ProductAttributeRelationId == request.Id && x.Id.NotIn(attributeRelationValueIds)).DeleteAsync();

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
                        .Set(x => x.PriceAdjustment, attributeValue.PriceAdjustment)
                        .Set(x => x.PricePercentageAdjustment, attributeValue.PricePercentageAdjustment)
                        .Set(x => x.Name, attributeValue.Name)
                        .Set(x => x.Quantity, attributeValue.Quantity)
                        .Set(x => x.DisplayOrder, attributeValue.DisplayOrder)
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

        public async Task CreateAttributeRelationValueAsync(long productAttributeRelationId, ProductAttributeRelationValueRequest attributeValue)
        {

            await _productAttributeRelationValueRepository.AddWithInt32EntityAsync(new ProductAttributeRelationValue
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
            await _productAttributeRelationValueRepository.Get(x => x.ProductAttributeRelationId.In(productAttributeRelationIds)).DeleteAsync();
            await productAttributeRelations.DeleteAsync();
        }

        public async Task<bool> DeleteAttributeRelationByAttributeIdAsync(int attributeId)
        {
            var productAttributeRelations = _productAttributeRelationRepository.Get(x => x.ProductAttributeId == attributeId);
            var productAttributeRelationIds = await productAttributeRelations.Select(x => x.Id).ToListAsync();
            await _productAttributeRelationValueRepository.Get(x => x.ProductAttributeRelationId.In(productAttributeRelationIds)).DeleteAsync();
            return await productAttributeRelations.DeleteAsync() > 0;
        }
    }
}