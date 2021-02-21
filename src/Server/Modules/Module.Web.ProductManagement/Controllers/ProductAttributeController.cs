using Camino.Shared.Requests.Filters;
using Camino.Core.Constants;
using Camino.Shared.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Core.Contracts.Helpers;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Services.Products;
using Camino.Shared.Requests.Products;

namespace Module.Web.ProductManagement.Controllers
{
    public class ProductAttributeController : BaseAuthController
    {
        private readonly IProductAttributeService _productAttributeService;
        private readonly IHttpHelper _httpHelper;

        public ProductAttributeController(IProductAttributeService productAttributeService,
            IHttpContextAccessor httpContextAccessor, IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _productAttributeService = productAttributeService;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProductAttribute)]
        [LoadResultAuthorizations("ProductAttribute", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(ProductAttributeFilterModel filter)
        {
            var filterRequest = new ProductAttributeFilter()
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search
            };

            var productAttributePageList = await _productAttributeService.GetAsync(filterRequest);
            var productAttributes = productAttributePageList.Collections.Select(x => new ProductAttributeModel()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name
            });

            var productAttributePage = new PageListModel<ProductAttributeModel>(productAttributes)
            {
                Filter = filter,
                TotalPage = productAttributePageList.TotalPage,
                TotalResult = productAttributePageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_ProductAttributeTable", productAttributePage);
            }

            return View(productAttributePage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProductAttribute)]
        [LoadResultAuthorizations("ProductAttribute", PolicyMethod.CanUpdate)]
        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var productAttribute = _productAttributeService.Find(id);
                if (productAttribute == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new ProductAttributeModel()
                {
                    Id = productAttribute.Id,
                    Description = productAttribute.Description,
                    Name = productAttribute.Name
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateProductAttribute)]
        public IActionResult Create()
        {
            var model = new ProductAttributeModel();
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateProductAttribute)]
        public async Task<IActionResult> Create(ProductAttributeModel model)
        {
            var productAttribute = new ProductAttributeModifyRequest()
            {
                Description = model.Description,
                Name = model.Name
            };

            var exist = _productAttributeService.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var id = await _productAttributeService.CreateAsync(productAttribute);

            return RedirectToAction(nameof(Detail), new { id });
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateProductAttribute)]
        public IActionResult Update(int id)
        {
            var exist = _productAttributeService.Find(id);
            var model = new ProductAttributeModel()
            {
                Id = exist.Id,
                Description = exist.Description,
                Name = exist.Name
            };
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateProductAttribute)]
        public async Task<IActionResult> Update(ProductAttributeModel model)
        {
            var productAttribute = new ProductAttributeModifyRequest()
            {
                Description = model.Description,
                Name = model.Name,
                Id = model.Id
            };

            if (productAttribute.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _productAttributeService.Find(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            await _productAttributeService.UpdateAsync(productAttribute);
            return RedirectToAction(nameof(Detail), new { id = productAttribute.Id });
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProductAttribute)]
        public async Task<IActionResult> Search(string q, string currentId = null)
        {
            var productAttributes = await _productAttributeService.SearchAsync(q);
            if (productAttributes == null || !productAttributes.Any())
            {
                return Json(new List<Select2ItemModel>());
            }

            var attributeSeletions = productAttributes
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Name
                });

            return Json(attributeSeletions);
        }
    }
}