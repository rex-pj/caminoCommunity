using Camino.Core.Constants;
using Camino.Core.Enums;
using Camino.Data.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Helpers.Contracts;
using Camino.Framework.Models;
using Camino.Service.Business.Products.Contracts;
using Camino.Service.Projections.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.ProductManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Web.ProductManagement.Controllers
{
    public class ProductPictureController : BaseAuthController
    {
        private readonly IProductPictureBusiness _productPictureBusiness;
        private readonly IHttpHelper _httpHelper;

        public ProductPictureController(IHttpContextAccessor httpContextAccessor, IProductPictureBusiness productPictureBusiness,
            IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _productPictureBusiness = productPictureBusiness;
            _httpHelper = httpHelper;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadPicture)]
        [LoadResultAuthorizations("Picture", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(ProductPictureFilterModel filter)
        {
            var filterRequest = new ProductPictureFilter()
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search,
                MimeType = filter.MimeType
            };

            var productPicturePageList = await _productPictureBusiness.GetAsync(filterRequest);
            var productPictures = productPicturePageList.Collections.Select(x => new ProductPictureModel
            {
                ProductName = x.ProductName,
                ProductId = x.ProductId,
                PictureId = x.PictureId,
                PictureName = x.PictureName,
                PictureCreatedBy = x.PictureCreatedBy,
                PictureCreatedById = x.PictureCreatedById,
                PictureCreatedDate = x.PictureCreatedDate,
                ProductPictureType = (ProductPictureType)x.ProductPictureType,
                ContentType = x.ContentType
            });

            var productPage = new PageListModel<ProductPictureModel>(productPictures)
            {
                Filter = filter,
                TotalPage = productPicturePageList.TotalPage,
                TotalResult = productPicturePageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_ProductPictureTable", productPage);
            }

            return View(productPage);
        }
    }
}
