using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Products;
using Camino.Application.Contracts.AppServices.Products.Dtos;
using Camino.Application.Contracts.Utils;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Products;
using Camino.Shared.Enums;
using Camino.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Camino.Application.AppServices.Products
{
    public class ProductAttributeAppService : IProductAttributeAppService, IScopedDependency
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IEntityRepository<ProductAttribute> _productAttributeEntityRepository;
        private readonly IEntityRepository<ProductAttributeRelation> _productAttributeRelationEntityRepository;
        private readonly IEntityRepository<ProductAttributeRelationValue> _productAttributeRelationValueEntityRepository;
        private readonly int _inactivedStatus = ProductAttributeStatuses.Inactived.GetCode();

        public ProductAttributeAppService(IProductAttributeRepository productAttributeRepository,
            IEntityRepository<ProductAttribute> productAttributeEntityRepository,
            IEntityRepository<ProductAttributeRelation> productAttributeRelationEntityRepository,
            IEntityRepository<ProductAttributeRelationValue> productAttributeRelationValueEntityRepository,
             IUserRepository userRepository)
        {
            _productAttributeRepository = productAttributeRepository;
            _productAttributeEntityRepository = productAttributeEntityRepository;
            _productAttributeRelationEntityRepository = productAttributeRelationEntityRepository;
            _productAttributeRelationValueEntityRepository = productAttributeRelationValueEntityRepository;
            _userRepository = userRepository;
        }

        #region get
        public async Task<ProductAttributeResult> FindAsync(IdRequestFilter<int> filter)
        {
            var productAttribute = await _productAttributeRepository.FindAsync(filter.Id);
            if (!filter.CanGetInactived && productAttribute.StatusId == _inactivedStatus)
            {
                return null;
            }

            return MapEntityToDto(productAttribute);
        }

        public async Task<ProductAttributeResult> FindByNameAsync(string name)
        {
            var productAttribute = await _productAttributeRepository.FindByNameAsync(name);
            return MapEntityToDto(productAttribute);
        }

        public async Task<BasePageList<ProductAttributeResult>> GetAsync(ProductAttributeFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var productAttributeQuery = _productAttributeEntityRepository.Get(x => filter.CanGetInactived || x.StatusId != _inactivedStatus);
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

            await PopulateModifiersAsync(productAttributes);
            var result = new BasePageList<ProductAttributeResult>(productAttributes)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task PopulateModifiersAsync(IList<ProductAttributeResult> productAttributes)
        {
            var createdByIds = productAttributes.Select(x => x.CreatedById).ToArray();
            var updatedByIds = productAttributes.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = await _userRepository.GetByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetByIdsAsync(updatedByIds);

            foreach (var category in productAttributes)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.UpdatedBy = updatedBy.DisplayName;
            }
        }

        public async Task<IList<ProductAttributeResult>> SearchAsync(ProductAttributeFilter filter)
        {
            string search = filter.Keyword;
            if (search == null)
            {
                search = string.Empty;
            }

            var inactivedStatus = ProductAttributeStatuses.Inactived.GetCode();
            search = search.ToLower();
            var query = _productAttributeEntityRepository.Get(x => filter.CanGetInactived || x.StatusId != _inactivedStatus);
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
                .Select(x => new ProductAttributeResult
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
        #endregion

        #region CRUD
        public async Task<int> CreateAsync(ProductAttributeModifyRequest request)
        {
            var newProductAttribute = new ProductAttribute
            {
                Description = request.Description,
                Name = request.Name,
                StatusId = ProductAttributeStatuses.Actived.GetCode(),
                CreatedById = request.CreatedById,
                UpdatedById = request.UpdatedById,
            };
            return await _productAttributeRepository.CreateAsync(newProductAttribute);
        }

        public async Task<bool> UpdateAsync(ProductAttributeModifyRequest request)
        {
            var existing = await _productAttributeRepository.FindAsync(request.Id);
            existing.Description = request.Description;
            existing.UpdatedById = request.UpdatedById;
            existing.Name = request.Name;
            return await _productAttributeRepository.UpdateAsync(existing); ;
        }

        public async Task<bool> ActiveAsync(ProductAttributeModifyRequest request)
        {
            var existing = await _productAttributeRepository.FindAsync(request.Id);
            existing.StatusId = (int)ProductAttributeStatuses.Actived;
            existing.UpdatedById = request.UpdatedById;
            return await _productAttributeRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeactivateAsync(ProductAttributeModifyRequest request)
        {
            var existing = await _productAttributeRepository.FindAsync(request.Id);
            existing.StatusId = (int)ProductAttributeStatuses.Inactived;
            existing.UpdatedById = request.UpdatedById;
            return await _productAttributeRepository.UpdateAsync(existing);
        }
        #endregion

        #region Control types
        public IList<SelectOption> GetAttributeControlTypes(ProductAttributeControlTypeFilter filter)
        {
            var result = SelectOptionUtils.ToSelectOptions<ProductAttributeControlTypes>().ToList();
            if (filter.ControlTypeId > 0)
            {
                result = result.Where(x => x.Id != filter.ControlTypeId.ToString()).ToList();
            }

            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
            return result;
        }
        #endregion

        #region category status
        public IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "")
        {
            search = search != null ? search.ToLower() : "";
            var result = new List<SelectOption>();
            if (filter.Id.HasValue)
            {
                var selected = (ProductAttributeStatuses)filter.Id;
                result = SelectOptionUtils.ToSelectOptions(selected).ToList();
            }
            else
            {
                result = SelectOptionUtils.ToSelectOptions<ProductAttributeStatuses>().ToList();
            }

            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
            return result;
        }

        public async Task<ProductAttributeRelationResult> GetAttributeRelationByIdAsync(long id)
        {
            var productAttribute = await (from pattr in _productAttributeRelationEntityRepository.Table
                                          join attr in _productAttributeEntityRepository.Table
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

        public async Task<IList<ProductAttributeRelationResult>> GetAttributeRelationsByProductIdAsync(long productId)
        {
            var productAttributes = await (from pattr in _productAttributeRelationEntityRepository.Table
                                           join attr in _productAttributeEntityRepository.Table
                                           on pattr.ProductAttributeId equals attr.Id

                                           join attrv in _productAttributeRelationValueEntityRepository.Table
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

        public async Task<ProductAttributeRelationValueResult> GetAttributeRelationValueByIdAsync(long id)
        {
            var productAttributeValue = await (from atv in _productAttributeRelationValueEntityRepository.Table
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
        #endregion

        private ProductAttributeResult MapEntityToDto(ProductAttribute entity)
        {
            return new ProductAttributeResult()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                StatusId = entity.StatusId
            };
        }
    }
}