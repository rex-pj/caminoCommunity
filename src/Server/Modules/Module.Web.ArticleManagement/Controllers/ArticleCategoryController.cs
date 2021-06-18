using Camino.Shared.Requests.Filters;
using Camino.Core.Constants;
using Camino.Shared.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.ArticleManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Services.Articles;
using Camino.Shared.Results.Articles;
using Camino.Shared.Requests.Articles;
using Camino.Core.Contracts.Helpers;

namespace Module.Web.ArticleManagement.Controllers
{
    public class ArticleCategoryController : BaseAuthController
    {
        private readonly IArticleCategoryService _articleCategoryService;
        private readonly IHttpHelper _httpHelper;

        public ArticleCategoryController(IArticleCategoryService articleCategoryService,
            IHttpContextAccessor httpContextAccessor, IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _articleCategoryService = articleCategoryService;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadArticleCategory)]
        [LoadResultAuthorizations("ArticleCategory", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(ArticleCategoryFilterModel filter)
        {
            var filterRequest = new ArticleCategoryFilter()
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search,
                UpdatedById = filter.UpdatedById
            };

            var categoryPageList = await _articleCategoryService.GetAsync(filterRequest);
            var categories = categoryPageList.Collections.Select(x => new ArticleCategoryModel
            {
                CreatedById = x.CreatedById,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                ParentCategoryName = x.ParentCategoryName,
                ParentId = x.ParentId,
                UpdateById = x.UpdatedById,
                UpdatedDate = x.UpdatedDate,
                StatusId = (ArticleCategoryStatus)x.StatusId
            });
            var categoryPage = new PageListModel<ArticleCategoryModel>(categories)
            {
                Filter = filter,
                TotalPage = categoryPageList.TotalPage,
                TotalResult = categoryPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_ArticleCategoryTable", categoryPage);
            }

            return View(categoryPage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadArticleCategory)]
        [LoadResultAuthorizations("ArticleCategory", PolicyMethod.CanUpdate)]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var category = await _articleCategoryService.FindAsync(id);
                if (category == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new ArticleCategoryModel
                {
                    Description = category.Description,
                    Id = category.Id,
                    ParentId = category.ParentId,
                    Name = category.Name,
                    UpdatedDate = category.UpdatedDate,
                    UpdateById = category.UpdatedById,
                    CreatedById = category.CreatedById,
                    CreatedDate = category.CreatedDate,
                    CreatedBy = category.CreatedBy,
                    UpdatedBy = category.UpdatedBy,
                    ParentCategoryName = category.ParentCategoryName,
                    StatusId = (ArticleCategoryStatus)category.StatusId
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateArticleCategory)]
        public IActionResult Create()
        {
            var model = new ArticleCategoryModel();
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateArticleCategory)]
        public async Task<IActionResult> Create(ArticleCategoryModel model)
        {
            var category = new ArticleCategoryModifyRequest
            {
                Description = model.Description,
                ParentId = model.ParentId,
                Name = model.Name,
                UpdatedById = LoggedUserId,
                CreatedById = LoggedUserId
            };
            var exist = await _articleCategoryService.FindByNameAsync(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            category.UpdatedById = LoggedUserId;
            category.CreatedById = LoggedUserId;
            var id = await _articleCategoryService.CreateAsync(category);

            return RedirectToAction(nameof(Detail), new { id });
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticleCategory)]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _articleCategoryService.FindAsync(id);
            var model = new ArticleCategoryModel
            {
                Description = category.Description,
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.Name,
                UpdatedDate = category.UpdatedDate,
                UpdateById = category.UpdatedById,
                CreatedById = category.CreatedById,
                CreatedDate = category.CreatedDate,
                ParentCategoryName = category.ParentCategoryName,
                StatusId = (ArticleCategoryStatus)category.StatusId
            };
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticleCategory)]
        public async Task<IActionResult> Update(ArticleCategoryModel model)
        {
            var category = new ArticleCategoryModifyRequest
            {
                Description = model.Description,
                ParentId = model.ParentId,
                Name = model.Name,
                UpdatedById = LoggedUserId,
                CreatedById = LoggedUserId,
                Id = model.Id
            };
            if (category.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = await _articleCategoryService.FindAsync(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            category.UpdatedById = LoggedUserId;
            await _articleCategoryService.UpdateAsync(category);
            return RedirectToAction(nameof(Detail), new { id = category.Id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticleCategory)]
        public async Task<IActionResult> Deactivate(ArticleCategoryIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isInactived = await _articleCategoryService.DeactiveAsync(new ArticleCategoryModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

            if (!isInactived)
            {
                return RedirectToErrorPage();
            }

            if (request.ShouldBackToDetail)
            {
                return RedirectToAction(nameof(Detail), new { id = request.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticleCategory)]
        public async Task<IActionResult> Active(ArticleCategoryIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _articleCategoryService.ActiveAsync(new ArticleCategoryModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

            if (!isActived)
            {
                return RedirectToErrorPage();
            }

            if (request.ShouldBackToDetail)
            {
                return RedirectToAction(nameof(Detail), new { id = request.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanDeleteArticleCategory)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _articleCategoryService.DeleteAsync(id);

            if (!isActived)
            {
                return RedirectToErrorPage();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadArticleCategory)]
        public IActionResult Search(string q, long? currentId = null, bool isParentOnly = false)
        {
            IList<ArticleCategoryResult> categories;
            if (isParentOnly)
            {
                categories = _articleCategoryService.SearchParents(q, currentId);
            }
            else
            {
                categories = _articleCategoryService.Search(q, currentId);
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
    }
}