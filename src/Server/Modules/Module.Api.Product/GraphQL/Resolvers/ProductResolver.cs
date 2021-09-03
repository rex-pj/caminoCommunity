using Camino.Framework.GraphQL.Resolvers;
using Camino.Framework.Models;
using Camino.Core.Domain.Identities;
using Camino.Core.Contracts.Services.Products;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Products;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using Module.Api.Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.IdentityManager;
using Camino.Shared.Requests.Products;
using Camino.Shared.Requests.Media;
using Camino.Core.Exceptions;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Camino.Shared.Configurations;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Module.Api.Product.GraphQL.Resolvers
{
    public class ProductResolver : BaseResolver, IProductResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IProductService _productService;
        private readonly PagerOptions _pagerOptions;

        public ProductResolver(IUserManager<ApplicationUser> userManager, IProductService productService, 
            IOptions<PagerOptions> pagerOptions)
            : base()
        {
            _userManager = userManager;
            _productService = productService;
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
                var productPageList = await _productService.GetAsync(filterRequest);
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
                var productPageList = await _productService.GetAsync(filterRequest);
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

            var filterRequest = new ProductFilter()
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize.HasValue && criterias.PageSize < _pagerOptions.PageSize ? criterias.PageSize.Value : _pagerOptions.PageSize,
                Keyword = criterias.Search
            };

            try
            {
                var relevantProducts = await _productService.GetRelevantsAsync(criterias.Id, filterRequest);
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
                CreatedByPhotoCode = x.CreatedByPhotoCode,
                Pictures = x.Pictures.Select(y => new PictureRequestModel()
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

        public async Task<ProductModel> GetProductAsync(ClaimsPrincipal claimsPrincipal, ProductFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ProductFilterModel();
            }

            try
            {
                var productResult = await _productService.FindDetailAsync(new IdRequestFilter<long>
                {
                    Id = criterias.Id,
                    CanGetInactived = true
                });

                var currentUserId = GetCurrentUserId(claimsPrincipal);
                if (currentUserId != productResult.CreatedById)
                {
                    throw new UnauthorizedAccessException();
                }

                var product = await MapProductResultToModelAsync(productResult);
                return product;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductModel> CreateProductAsync(ClaimsPrincipal claimsPrincipal, ProductModel criterias)
        {
            var currentUserId = GetCurrentUserId(claimsPrincipal);
            var product = new ProductModifyRequest()
            {
                CreatedById = currentUserId,
                UpdatedById = currentUserId,
                Name = criterias.Name,
                Description = criterias.Description,
                Price = criterias.Price,
                Pictures = criterias.Pictures.Select(x => new PictureRequest
                {
                    Base64Data = x.Base64Data,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                }),
                Categories = criterias.Categories.Select(x => new ProductCategoryRequest
                {
                    Id = x.Id
                }),
                Farms = criterias.Farms.Select(x => new ProductFarmRequest
                {
                    FarmId = x.Id
                }),
                ProductAttributes = criterias.ProductAttributes.Select(x => new ProductAttributeRelationRequest
                {
                    ControlTypeId = x.ControlTypeId,
                    DisplayOrder = x.DisplayOrder,
                    ProductAttributeId = x.AttributeId,
                    AttributeRelationValues = x.AttributeRelationValues.Select(c => new ProductAttributeRelationValueRequest
                    {
                        DisplayOrder = c.DisplayOrder,
                        Name = c.Name,
                        PriceAdjustment = c.PriceAdjustment,
                        PricePercentageAdjustment = c.PricePercentageAdjustment,
                        Quantity = c.Quantity
                    })
                })
            };

            criterias.Id = await _productService.CreateAsync(product);
            return criterias;
        }

        public async Task<ProductModel> UpdateProductAsync(ClaimsPrincipal claimsPrincipal, ProductModel criterias)
        {
            var exist = await _productService.FindAsync(new IdRequestFilter<long>
            {
                Id = criterias.Id,
                CanGetInactived = true
            });

            if (exist == null)
            {
                throw new CaminoApplicationException("No article found");
            }

            var currentUserId = GetCurrentUserId(claimsPrincipal);
            if (currentUserId != exist.CreatedById)
            {
                throw new UnauthorizedAccessException();
            }

            var product = new ProductModifyRequest()
            {
                Id = criterias.Id,
                CreatedById = currentUserId,
                UpdatedById = currentUserId,
                Name = criterias.Name,
                Description = criterias.Description,
                Price = criterias.Price,
                Pictures = criterias.Pictures.Select(x => new PictureRequest()
                {
                    Base64Data = x.Base64Data,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                    Id = x.PictureId
                }),
                Categories = criterias.Categories.Select(x => new ProductCategoryRequest()
                {
                    Id = x.Id
                }),
                Farms = criterias.Farms.Select(x => new ProductFarmRequest()
                {
                    FarmId = x.Id
                }),
                ProductAttributes = criterias.ProductAttributes?.Select(x => new ProductAttributeRelationRequest
                {
                    Id = x.Id,
                    ControlTypeId = x.ControlTypeId,
                    DisplayOrder = x.DisplayOrder,
                    ProductAttributeId = x.AttributeId,
                    AttributeRelationValues = x.AttributeRelationValues?.Select(c => new ProductAttributeRelationValueRequest
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

            await _productService.UpdateAsync(product);
            return criterias;
        }

        public async Task<bool> DeleteProductAsync(ClaimsPrincipal claimsPrincipal, ProductFilterModel criterias)
        {
            try
            {
                var exist = await _productService.FindAsync(new IdRequestFilter<long>
                {
                    Id = criterias.Id,
                    CanGetInactived = true
                });

                if (exist == null)
                {
                    return false;
                }

                var currentUserId = GetCurrentUserId(claimsPrincipal);
                if (currentUserId != exist.CreatedById)
                {
                    throw new UnauthorizedAccessException();
                }

                return await _productService.SoftDeleteAsync(new ProductModifyRequest
                {
                    UpdatedById = currentUserId,
                    Id = criterias.Id
                });
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
                CreatedByPhotoCode = productResult.CreatedByPhotoCode,
                Categories = productResult.Categories.Select(x => new ProductCategoryRelationModel()
                {
                    Id = x.Id,
                    Name = x.Name
                }),
                Farms = productResult.Farms.Select(x => new ProductFarmModel()
                {
                    Name = x.Name,
                    Id = x.FarmId
                }),
                Pictures = productResult.Pictures.Select(y => new PictureRequestModel()
                {
                    PictureId = y.Id
                }),
                ProductAttributes = productResult.ProductAttributes.Select(x => new ProductAttributeRelationModel
                {
                    AttributeId = x.AttributeId,
                    ControlTypeId = x.AttributeControlTypeId,
                    DisplayOrder = x.DisplayOrder,
                    Id = x.Id,
                    IsRequired = x.IsRequired,
                    TextPrompt = x.TextPrompt,
                    Name = x.AttributeName,
                    ControlTypeName = ((ProductAttributeControlType)x.AttributeControlTypeId).GetEnumDescription(),
                    AttributeRelationValues = x.AttributeRelationValues.Select(c => new ProductAttributeRelationValueModel
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
