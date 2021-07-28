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
            var productAttributePageList = await _productAttributeService.GetAsync(new ProductAttributeFilter
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search,
                CanGetInactived = true,
                StatusId = filter.StatusId
            });
            var productAttributes = productAttributePageList.Collections.Select(x => new ProductAttributeModel
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                CreatedById = x.CreatedById,
                UpdatedById = x.UpdatedById,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate,
                StatusId = (ProductAttributeStatus)x.StatusId,
                UpdatedBy = x.UpdatedBy,
                CreatedBy = x.CreatedBy
            });

            var productAttributePage = new PageListModel<ProductAttributeModel>(productAttributes)
            {
                Filter = filter,
                TotalPage = productAttributePageList.TotalPage,
                TotalResult = productAttributePageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("Partial/_ProductAttributeTable", productAttributePage);
            }

            return View(productAttributePage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProductAttribute)]
        [LoadResultAuthorizations("ProductAttribute", PolicyMethod.CanUpdate)]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var productAttribute = await _productAttributeService.FindAsync(new IdRequestFilter<int>
                {
                    Id = id,
                    CanGetInactived = true
                });
                if (productAttribute == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new ProductAttributeModel()
                {
                    Id = productAttribute.Id,
                    Description = productAttribute.Description,
                    Name = productAttribute.Name,
                    StatusId = (ProductAttributeStatus)productAttribute.StatusId
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
                Name = model.Name,
                CreatedById = LoggedUserId,
                UpdatedById = LoggedUserId
            };

            var exist = await _productAttributeService.FindByNameAsync(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var id = await _productAttributeService.CreateAsync(productAttribute);

            return RedirectToAction(nameof(Detail), new { id });
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateProductAttribute)]
        public async Task<IActionResult> Update(int id)
        {
            var exist = await _productAttributeService.FindAsync(new IdRequestFilter<int>
            {
                Id = id,
                CanGetInactived = true
            });
            var model = new ProductAttributeModel
            {
                Id = exist.Id,
                Description = exist.Description,
                Name = exist.Name,
                StatusId = (ProductAttributeStatus)exist.StatusId
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
                Id = model.Id,
                UpdatedById = LoggedUserId
            };

            if (productAttribute.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _productAttributeService.FindAsync(new IdRequestFilter<int>
            {
                Id = model.Id,
                CanGetInactived = true
            });
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            await _productAttributeService.UpdateAsync(productAttribute);
            return RedirectToAction(nameof(Detail), new { id = productAttribute.Id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateProductAttribute)]
        public async Task<IActionResult> Deactivate(ProductAttributeIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isInactived = await _productAttributeService.DeactivateAsync(new ProductAttributeModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

            if (!isInactived)
            {
                return RedirectToErrorPage();
            }

            if (request.ShouldKeepUpdatePage)
            {
                return RedirectToAction(nameof(Update), new { id = request.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateProductAttribute)]
        public async Task<IActionResult> Active(ProductAttributeIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _productAttributeService.ActiveAsync(new ProductAttributeModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

            if (!isActived)
            {
                return RedirectToErrorPage();
            }

            if (request.ShouldKeepUpdatePage)
            {
                return RedirectToAction(nameof(Update), new { id = request.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanDeleteProductAttribute)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _productAttributeService.DeleteAsync(id);

            if (!isActived)
            {
                return RedirectToErrorPage();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProductAttribute)]
        public async Task<IActionResult> Search(string q, int? currentId = null, string excluded = null)
        {
            var excludedIds = new List<int>();
            if (!string.IsNullOrWhiteSpace(excluded))
            {
                excludedIds = excluded.Split(',').Select(int.Parse).ToList();
            }

            var productAttributes = await _productAttributeService.SearchAsync(new ProductAttributeFilter
            {
                Search = q,
                Id = currentId,
                ExcludedIds = excludedIds
            });
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

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProductAttribute)]
        public IActionResult SearchControlTypes(string q, int? currentId = null)
        {
            var controlTypeId = currentId.HasValue ? currentId.Value : 0;
            var productAttributes = _productAttributeService.GetAttributeControlTypes(new ProductAttributeControlTypeFilter
            {
                Search = q,
                ControlTypeId = controlTypeId
            });
            if (productAttributes == null || !productAttributes.Any())
            {
                return Json(new List<Select2ItemModel>());
            }

            var attributeSeletions = productAttributes
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Text
                });

            return Json(attributeSeletions);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProductAttribute)]
        public IActionResult SearchStatus(string q, int? currentId = null)
        {
            var statuses = _productAttributeService.SearchStatus(new IdRequestFilter<int?>
            {
                Id = currentId
            }, q);

            if (statuses == null || !statuses.Any())
            {
                return Json(new List<Select2ItemModel>());
            }

            var categorySeletions = statuses
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Text
                });

            return Json(categorySeletions);
        }
    }
}