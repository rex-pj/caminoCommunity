using Camino.Shared.Enums;
using Camino.Infrastructure.AspNetCore.Controllers;
using Camino.Infrastructure.AspNetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Camino.Application.Contracts.AppServices.Products;
using Camino.Infrastructure.Http.Interfaces;
using Camino.Shared.Configuration.Options;
using Camino.Shared.Constants;
using Camino.Application.Contracts.AppServices.Products.Dtos;
using Camino.Application.Contracts;
using Camino.Core.Domains.Products.DomainServices;
using Camino.Infrastructure.Identity.Attributes;

namespace Module.Web.ProductManagement.Controllers
{
    public class ProductAttributeController : BaseAuthController
    {
        private readonly IProductAttributeAppService _productAttributeAppService;
        private readonly IProductAttributeDomainService _productAttributeDomainService;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public ProductAttributeController(IProductAttributeAppService productAttributeAppService,
            IProductAttributeDomainService productAttributeDomainService,
            IHttpContextAccessor httpContextAccessor, IHttpHelper httpHelper, IOptions<PagerOptions> pagerOptions)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _productAttributeAppService = productAttributeAppService;
            _productAttributeDomainService = productAttributeDomainService;
            _pagerOptions = pagerOptions.Value;
        }

        [ApplicationAuthorize(AuthorizePolicies.CanReadProductAttribute)]
        [PopulatePermissions("ProductAttribute", PolicyMethods.CanCreate, PolicyMethods.CanUpdate, PolicyMethods.CanDelete)]
        public async Task<IActionResult> Index(ProductAttributeFilterModel filter)
        {
            var productAttributePageList = await _productAttributeAppService.GetAsync(new ProductAttributeFilter
            {
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search,
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
                StatusId = (ProductAttributeStatuses)x.StatusId,
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

        [ApplicationAuthorize(AuthorizePolicies.CanReadProductAttribute)]
        [PopulatePermissions("ProductAttribute", PolicyMethods.CanUpdate)]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var productAttribute = await _productAttributeAppService.FindAsync(new IdRequestFilter<int>
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
                    StatusId = (ProductAttributeStatuses)productAttribute.StatusId
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicies.CanCreateProductAttribute)]
        public IActionResult Create()
        {
            var model = new ProductAttributeModel();
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanCreateProductAttribute)]
        public async Task<IActionResult> Create(ProductAttributeModel model)
        {
            var productAttribute = new ProductAttributeModifyRequest()
            {
                Description = model.Description,
                Name = model.Name,
                CreatedById = LoggedUserId,
                UpdatedById = LoggedUserId
            };

            var exist = await _productAttributeAppService.FindByNameAsync(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var id = await _productAttributeAppService.CreateAsync(productAttribute);

            return RedirectToAction(nameof(Detail), new { id });
        }

        [ApplicationAuthorize(AuthorizePolicies.CanUpdateProductAttribute)]
        public async Task<IActionResult> Update(int id)
        {
            var exist = await _productAttributeAppService.FindAsync(new IdRequestFilter<int>
            {
                Id = id,
                CanGetInactived = true
            });
            var model = new ProductAttributeModel
            {
                Id = exist.Id,
                Description = exist.Description,
                Name = exist.Name,
                StatusId = (ProductAttributeStatuses)exist.StatusId
            };
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateProductAttribute)]
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

            var exist = _productAttributeAppService.FindAsync(new IdRequestFilter<int>
            {
                Id = model.Id,
                CanGetInactived = true
            });
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            await _productAttributeAppService.UpdateAsync(productAttribute);
            return RedirectToAction(nameof(Detail), new { id = productAttribute.Id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateProductAttribute)]
        public async Task<IActionResult> Deactivate(ProductAttributeIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isInactived = await _productAttributeAppService.DeactivateAsync(new ProductAttributeModifyRequest
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
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateProductAttribute)]
        public async Task<IActionResult> Active(ProductAttributeIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _productAttributeAppService.ActiveAsync(new ProductAttributeModifyRequest
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
        [ApplicationAuthorize(AuthorizePolicies.CanDeleteProductAttribute)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _productAttributeDomainService.DeleteAsync(id);

            if (!isActived)
            {
                return RedirectToErrorPage();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanReadProductAttribute)]
        public async Task<IActionResult> Search(string q, int? currentId = null, string excluded = null)
        {
            var excludedIds = new List<int>();
            if (!string.IsNullOrWhiteSpace(excluded))
            {
                excludedIds = excluded.Split(',').Select(int.Parse).ToList();
            }

            var productAttributes = await _productAttributeAppService.SearchAsync(new ProductAttributeFilter
            {
                Keyword = q,
                Id = currentId,
                ExcludedIds = excludedIds,
                PageSize = _pagerOptions.PageSize,
                Page = _defaultPageSelection
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
        [ApplicationAuthorize(AuthorizePolicies.CanReadProductAttribute)]
        public IActionResult SearchControlTypes(string q, int? currentId = null)
        {
            var controlTypeId = currentId.HasValue ? currentId.Value : 0;
            var productAttributes = _productAttributeAppService.GetAttributeControlTypes(new ProductAttributeControlTypeFilter
            {
                Keyword = q,
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
        [ApplicationAuthorize(AuthorizePolicies.CanReadProductAttribute)]
        public IActionResult SearchStatus(string q, int? currentId = null)
        {
            var statuses = _productAttributeAppService.SearchStatus(new IdRequestFilter<int?>
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