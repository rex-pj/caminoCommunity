using Camino.Framework.GraphQL.Resolvers;
using Camino.Framework.Models;
using Camino.IdentityManager.Contracts;
using Camino.IdentityManager.Contracts.Core;
using Camino.IdentityManager.Models;
using Camino.Service.Business.Products.Contracts;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Media;
using Camino.Service.Projections.Product;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using Module.Api.Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Api.Product.GraphQL.Resolvers
{
    public class ProductResolver : BaseResolver, IProductResolver
    {
        private readonly IProductBusiness _productBusiness;
        private readonly IUserManager<ApplicationUser> _userManager;

        public ProductResolver(SessionState sessionState, IProductBusiness productBusiness, IUserManager<ApplicationUser> userManager)
            : base(sessionState)
        {
            _productBusiness = productBusiness;
            _userManager = userManager;
        }

        public async Task<ProductModel> CreateProductAsync(ProductModel criterias)
        {
            var product = new ProductProjection()
            {
                CreatedById = CurrentUser.Id,
                UpdatedById = CurrentUser.Id,
                Name = criterias.Name,
                Description = criterias.Description,
                Price = criterias.Price,
                Thumbnails = criterias.Thumbnails.Select(x => new PictureRequestProjection()
                {
                    Base64Data = x.Base64Data,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                }),
                ProductCategories = criterias.ProductCategories.Select(x => new ProductCategoryProjection()
                {
                    Id = x.Id
                }),
                ProductFarms = criterias.ProductFarms.Select(x => new ProductFarmProjection()
                {
                    FarmId = x.Id
                })
            };

            var id = await _productBusiness.CreateAsync(product);
            criterias.Id = id;
            return criterias;
        }

        public async Task<ProductPageListModel> GetUserProductsAsync(ProductFilterModel criterias)
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

            var userId = await _userManager.DecryptUserIdAsync(criterias.UserIdentityId);
            var filterRequest = new ProductFilter()
            {
                Page = criterias.Page,
                PageSize = criterias.PageSize,
                Search = criterias.Search,
                CreatedById = userId
            };

            try
            {
                var productPageList = await _productBusiness.GetAsync(filterRequest);
                var products = await MapProductsProjectionToModelAsync(productPageList.Collections);

                var productPage = new ProductPageListModel(products)
                {
                    Filter = criterias,
                    TotalPage = productPageList.TotalPage,
                    TotalResult = productPageList.TotalResult
                };

                return productPage;
            }
            catch (Exception e)
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
                PageSize = criterias.PageSize,
                Search = criterias.Search
            };

            try
            {
                var productPageList = await _productBusiness.GetAsync(filterRequest);
                var products = await MapProductsProjectionToModelAsync(productPageList.Collections);

                var productPage = new ProductPageListModel(products)
                {
                    Filter = criterias,
                    TotalPage = productPageList.TotalPage,
                    TotalResult = productPageList.TotalResult
                };

                return productPage;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task<IList<ProductModel>> MapProductsProjectionToModelAsync(IEnumerable<ProductProjection> productProjections)
        {
            var products = productProjections.Select(x => new ProductModel()
            {
                Id = x.Id,
                CreatedBy = x.CreatedBy,
                CreatedById = x.CreatedById,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                Name = x.Name,
                Price = x.Price,
                CreatedByPhotoCode = x.CreatedByPhotoCode,
                Thumbnails = x.Thumbnails.Select(y => new PictureRequestModel()
                {
                    Id = y.Id
                }),
                ProductFarms = x.ProductFarms.Select(x => new ProductFarmModel()
                {
                    FarmName = x.FarmName,
                    Id = x.Id,
                    FarmId = x.FarmId
                })
            }).ToList();

            foreach (var product in products)
            {
                product.CreatedByIdentityId = await _userManager.EncryptUserIdAsync(product.CreatedById);
            }

            return products;
        }

        public async Task<ProductModel> GetProductAsync(ProductFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ProductFilterModel();
            }

            try
            {
                var productProjection = await _productBusiness.FindDetailAsync(criterias.Id);

                var product = await MapProductProjectionToModelAsync(productProjection);

                return product;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task<ProductModel> MapProductProjectionToModelAsync(ProductProjection productProjection)
        {
            var product = new ProductModel()
            {
                Id = productProjection.Id,
                CreatedBy = productProjection.CreatedBy,
                CreatedById = productProjection.CreatedById,
                CreatedDate = productProjection.CreatedDate,
                Description = productProjection.Description,
                Name = productProjection.Name,
                Price = productProjection.Price,
                Thumbnails = productProjection.Thumbnails.Select(y => new PictureRequestModel()
                {
                    Id = y.Id
                }),
                CreatedByPhotoCode = productProjection.CreatedByPhotoCode,
                ProductFarms = productProjection.ProductFarms.Select(x => new ProductFarmModel()
                {
                    FarmName = x.FarmName,
                    Id = x.Id,
                    FarmId = x.FarmId
                })
            };

            product.CreatedByIdentityId = await _userManager.EncryptUserIdAsync(product.CreatedById);

            return product;
        }
    }
}
