using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.Enums;
using Microsoft.Extensions.Options;
using Camino.Application.Contracts.AppServices.Products;
using Camino.Infrastructure.Http.Interfaces;
using Camino.Shared.Configuration.Options;
using Camino.Shared.Constants;
using Camino.Application.Contracts.AppServices.Products.Dtos;
using Camino.Application.Contracts;

namespace Module.Web.ProductManagement.Controllers
{
    public class ProductCategoryController : BaseAuthController
    {
        private readonly IProductCategoryAppService _productCategoryAppService;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public ProductCategoryController(IProductCategoryAppService productCategoryAppService,
            IHttpContextAccessor httpContextAccessor, IHttpHelper httpHelper, IOptions<PagerOptions> pagerOptions)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _productCategoryAppService = productCategoryAppService;
            _pagerOptions = pagerOptions.Value;
        }

        [ApplicationAuthorize(AuthorizePolicies.CanReadProductCategory)]
        [LoadResultAuthorizations("ProductCategory", PolicyMethods.CanCreate, PolicyMethods.CanUpdate, PolicyMethods.CanDelete)]
        public async Task<IActionResult> Index(ProductCategoryFilterModel filter)
        {
            var categoryPageList = await _productCategoryAppService.GetAsync(new ProductCategoryFilter
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search,
                UpdatedById = filter.UpdatedById,
                StatusId = filter.StatusId
            });
            var categories = categoryPageList.Collections.Select(x => new ProductCategoryModel()
            {
                Id = x.Id,
                Description = x.Description,
                CreatedBy = x.CreatedBy,
                CreatedById = x.CreatedById,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                Name = x.Name,
                StatusId = (ProductCategoryStatuses)x.StatusId
            });

            var categoryPage = new PageListModel<ProductCategoryModel>(categories)
            {
                Filter = filter,
                TotalPage = categoryPageList.TotalPage,
                TotalResult = categoryPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("Partial/_ProductCategoryTable", categoryPage);
            }

            return View(categoryPage);
        }

        [ApplicationAuthorize(AuthorizePolicies.CanReadProductCategory)]
        [LoadResultAuthorizations("ProductCategory", PolicyMethods.CanUpdate)]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var category = await _productCategoryAppService.FindAsync(id);
                if (category == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new ProductCategoryModel()
                {
                    Id = category.Id,
                    Description = category.Description,
                    CreatedBy = category.CreatedBy,
                    CreatedById = category.CreatedById,
                    CreatedDate = category.CreatedDate,
                    Name = category.Name,
                    UpdateById = category.UpdatedById,
                    ParentId = category.ParentId,
                    ParentCategoryName = category.ParentCategoryName,
                    UpdatedBy = category.UpdatedBy,
                    UpdatedDate = category.UpdatedDate,
                    StatusId = (ProductCategoryStatuses)category.StatusId
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicies.CanCreateProductCategory)]
        public IActionResult Create()
        {
            var model = new ProductCategoryModel();
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanCreateProductCategory)]
        public async Task<IActionResult> Create(ProductCategoryModel model)
        {
            var exist = await _productCategoryAppService.FindByNameAsync(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var category = new ProductCategoryRequest
            {
                Description = model.Description,
                Name = model.Name,
                ParentId = model.ParentId,
                UpdatedById = LoggedUserId,
                CreatedById = LoggedUserId
            };

            var id = await _productCategoryAppService.CreateAsync(category);
            return RedirectToAction(nameof(Detail), new { id });
        }

        [ApplicationAuthorize(AuthorizePolicies.CanUpdateProductCategory)]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _productCategoryAppService.FindAsync(id);
            var model = new ProductCategoryModel()
            {
                Id = category.Id,
                Description = category.Description,
                CreatedBy = category.CreatedBy,
                CreatedById = category.CreatedById,
                CreatedDate = category.CreatedDate,
                Name = category.Name,
                UpdateById = category.UpdatedById,
                ParentId = category.ParentId,
                ParentCategoryName = category.ParentCategoryName,
                UpdatedBy = category.UpdatedBy,
                UpdatedDate = category.UpdatedDate,
                StatusId = (ProductCategoryStatuses)category.StatusId
            };
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateProductCategory)]
        public async Task<IActionResult> Update(ProductCategoryModel model)
        {
            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = await _productCategoryAppService.FindAsync(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            var category = new ProductCategoryRequest()
            {
                Description = model.Description,
                Name = model.Name,
                ParentId = model.ParentId,
                Id = model.Id,
                UpdatedById = LoggedUserId
            };

            await _productCategoryAppService.UpdateAsync(category);
            return RedirectToAction(nameof(Detail), new { id = category.Id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateProductCategory)]
        public async Task<IActionResult> Deactivate(ProductCategoryIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isInactived = await _productCategoryAppService.DeactivateAsync(new ProductCategoryRequest
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
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateProductCategory)]
        public async Task<IActionResult> Active(ProductCategoryIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _productCategoryAppService.ActiveAsync(new ProductCategoryRequest
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
        [ApplicationAuthorize(AuthorizePolicies.CanDeleteProductCategory)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _productCategoryAppService.DeleteAsync(id);

            if (!isActived)
            {
                return RedirectToErrorPage();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanReadProductCategory)]
        public async Task<IActionResult> Search(string q, string currentId = null, bool isParentOnly = false)
        {
            long[] currentIds = null;
            if (!string.IsNullOrEmpty(currentId))
            {
                currentIds = currentId.Split(',').Select(x => long.Parse(x)).ToArray();
            }

            IList<ProductCategoryResult> categories;
            var filter = new BaseFilter
            {
                Keyword = q,
                PageSize = _pagerOptions.PageSize,
                Page = _defaultPageSelection
            };
            if (isParentOnly)
            {
                categories = await _productCategoryAppService.SearchParentsAsync(filter, currentIds);
            }
            else
            {
                categories = await _productCategoryAppService.SearchAsync(filter, currentIds);
            }

            if (categories == null || !categories.Any())
            {
                return Json(new List<Select2ItemModel>());
            }

            var categorySeletions = categories
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.ParentId.HasValue ? $"-- {x.Name}" : x.Name
                });

            return Json(categorySeletions);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanReadProductCategory)]
        public IActionResult SearchStatus(string q, int? currentId = null)
        {
            var statuses = _productCategoryAppService.SearchStatus(new IdRequestFilter<int?>
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