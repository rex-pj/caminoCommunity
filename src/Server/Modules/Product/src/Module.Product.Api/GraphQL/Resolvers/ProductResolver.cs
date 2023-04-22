using Camino.Infrastructure.GraphQL.Resolvers;
using Camino.Infrastructure.AspNetCore.Models;
using Module.Product.Api.GraphQL.Resolvers.Contracts;
using Module.Product.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Application.Contracts.AppServices.Products;
using Camino.Shared.Configuration.Options;
using Camino.Application.Contracts.AppServices.Products.Dtos;
using Camino.Application.Contracts;
using Camino.Shared.Enums;
using Camino.Application.Contracts.Utils;

namespace Module.Product.Api.GraphQL.Resolvers
{
    public class ProductResolver : BaseResolver, IProductResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IProductAppService _productAppService;
        private readonly PagerOptions _pagerOptions;

        public ProductResolver(IUserManager<ApplicationUser> userManager, IProductAppService productAppService,
            IOptions<PagerOptions> pagerOptions)
            : base()
        {
            _userManager = userManager;
            _productAppService = productAppService;
            _pagerOptions = pagerOptions.Value;
        }

        public async Task<ProductPageListModel> GetUserProductsAsync(ClaimsPrincipal claimsPrincipal, ProductFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ProductFilterModel();
            }

            if (string.IsNullOrEmpty(criterias.UserIdentityId))
            {
                return new ProductPageListModel(new List<ProductModel>())
                {
                    Filter = criterias
                };
            }

            var currentUserId = GetCurrentUserId(claimsPrincipal);
            var userId = await _userManager.DecryptUserIdAsync(criterias.UserIdentityId);
            var filterRequest = new ProductFilter()
            {
                Page = criterias.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = criterias.Search,
                CreatedById = userId,
                CanGetInactived = currentUserId == userId
            };

            try
            {
                var productPageList = await _productAppService.GetAsync(filterRequest);
                var products = await MapProductsResultToModelAsync(productPageList.Collections);

                var productPage = new ProductPageListModel(products)
                {
                    Filter = criterias,
                    TotalPage = productPageList.TotalPage,
                    TotalResult = productPageList.TotalResult
                };

                return productPage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductPageListModel> GetProductsAsync(ProductFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ProductFilterModel();
            }

            var filterRequest = new ProductFilter()
            {
                Page = criterias.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = criterias.Search,
                FarmId = criterias.FarmId
            };

            try
            {
                var productPageList = await _productAppService.GetAsync(filterRequest);
                var products = await MapProductsResultToModelAsync(productPageList.Collections);

                var productPage = new ProductPageListModel(products)
                {
                    Filter = criterias,
                    TotalPage = productPageList.TotalPage,
                    TotalResult = productPageList.TotalResult
                };

                return productPage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<ProductModel>> GetRelevantProductsAsync(ProductFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ProductFilterModel();
            }

            if (!criterias.Id.HasValue || criterias.Id <= 0)
            {
                return new List<ProductModel>();
            }

            var filterRequest = new ProductFilter()
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize.HasValue && criterias.PageSize < _pagerOptions.PageSize ? criterias.PageSize.Value : _pagerOptions.PageSize,
                Keyword = criterias.Search
            };

            try
            {
                var relevantProducts = await _productAppService.GetRelevantsAsync(criterias.Id.GetValueOrDefault(), filterRequest);
                var products = await MapProductsResultToModelAsync(relevantProducts);

                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<IList<ProductModel>> MapProductsResultToModelAsync(IEnumerable<ProductResult> productResults)
        {
            var products = productResults.Select(x => new ProductModel()
            {
                Id = x.Id,
                CreatedBy = x.CreatedBy,
                CreatedById = x.CreatedById,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                Name = x.Name,
                Price = x.Price,
                CreatedByPhotoId = x.CreatedByPhotoId,
                Pictures = x.Pictures.Select(y => new PictureResultModel()
                {
                    PictureId = y.Id
                }),
                Farms = x.Farms.Select(x => new ProductFarmModel()
                {
                    Name = x.Name,
                    Id = x.FarmId
                })
            }).ToList();

            foreach (var product in products)
            {
                product.CreatedByIdentityId = await _userManager.EncryptUserIdAsync(product.CreatedById);
            }

            return products;
        }

        public async Task<ProductModel> GetProductAsync(ProductIdFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ProductIdFilterModel();
            }

            if (criterias.Id <= 0)
            {
                return new ProductModel();
            }

            try
            {
                var productResult = await _productAppService.FindDetailAsync(new IdRequestFilter<long>
                {
                    Id = criterias.Id,
                    CanGetInactived = true
                });


                var product = await MapProductResultToModelAsync(productResult);
                return product;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<ProductModel> MapProductResultToModelAsync(ProductResult productResult)
        {
            var product = new ProductModel()
            {
                Id = productResult.Id,
                CreatedBy = productResult.CreatedBy,
                CreatedById = productResult.CreatedById,
                CreatedDate = productResult.CreatedDate,
                Description = productResult.Description,
                Name = productResult.Name,
                Price = productResult.Price,
                CreatedByPhotoId = productResult.CreatedByPhotoId,
                Categories = productResult.Categories.Select(x => new ProductCategoryRelationModel
                {
                    Id = x.Id,
                    Name = x.Name
                }),
                Farms = productResult.Farms.Select(x => new ProductFarmModel
                {
                    Name = x.Name,
                    Id = x.FarmId
                }),
                Pictures = productResult.Pictures.Select(y => new PictureResultModel
                {
                    PictureId = y.Id
                }),
                ProductAttributes = productResult.ProductAttributes.Select(x => new AttributeRelationResultModel
                {
                    AttributeId = x.AttributeId,
                    ControlTypeId = x.AttributeControlTypeId,
                    DisplayOrder = x.DisplayOrder,
                    Id = x.Id,
                    IsRequired = x.IsRequired,
                    TextPrompt = x.TextPrompt,
                    Name = x.AttributeName,
                    ControlTypeName = ((ProductAttributeControlTypes)x.AttributeControlTypeId).GetEnumDescription(),
                    AttributeRelationValues = x.AttributeRelationValues.Select(c => new AttributeRelationValueResultModel
                    {
                        Id = c.Id,
                        DisplayOrder = c.DisplayOrder,
                        Name = c.Name,
                        PriceAdjustment = c.PriceAdjustment,
                        PricePercentageAdjustment = c.PricePercentageAdjustment,
                        Quantity = c.Quantity
                    })
                })
            };

            product.CreatedByIdentityId = await _userManager.EncryptUserIdAsync(product.CreatedById);

            return product;
        }
    }
}
