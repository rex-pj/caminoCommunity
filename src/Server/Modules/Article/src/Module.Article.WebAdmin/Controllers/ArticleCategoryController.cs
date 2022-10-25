using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Article.WebAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Camino.Infrastructure.Http.Interfaces;
using Camino.Shared.Configuration.Options;
using Camino.Infrastructure.AspNetCore.Controllers;
using Camino.Application.Contracts.AppServices.Articles;
using Camino.Infrastructure.Identity.Attributes;
using Camino.Shared.Constants;
using Camino.Shared.Enums;
using Camino.Application.Contracts.AppServices.Articles.Dtos;
using Camino.Infrastructure.AspNetCore.Models;
using Camino.Application.Contracts;

namespace Module.Article.WebAdmin.Controllers
{
    public class ArticleCategoryController : BaseAuthController
    {
        private readonly IArticleCategoryAppService _articleCategoryAppService;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public ArticleCategoryController(IArticleCategoryAppService articleCategoryAppService,
            IHttpContextAccessor httpContextAccessor, IHttpHelper httpHelper, IOptions<PagerOptions> pagerOptions)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _articleCategoryAppService = articleCategoryAppService;
            _pagerOptions = pagerOptions.Value;
        }

        [ApplicationAuthorize(AuthorizePolicies.CanReadArticleCategory)]
        [PopulatePermissions("ArticleCategory", PolicyMethods.CanCreate, PolicyMethods.CanUpdate, PolicyMethods.CanDelete)]
        public async Task<IActionResult> Index(ArticleCategoryFilterModel filter)
        {
            var categoryPageList = await _articleCategoryAppService.GetAsync(new ArticleCategoryFilter
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
            var categories = categoryPageList.Collections.Select(x => new ArticleCategoryModel
            {
                CreatedById = x.CreatedById,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                ParentCategoryName = x.ParentCategoryName,
                ParentId = x.ParentId,
                UpdateById = x.UpdatedById,
                UpdatedDate = x.UpdatedDate,
                UpdatedBy = x.UpdatedBy,
                StatusId = (ArticleCategoryStatuses)x.StatusId
            });
            var categoryPage = new PageListModel<ArticleCategoryModel>(categories)
            {
                Filter = filter,
                TotalPage = categoryPageList.TotalPage,
                TotalResult = categoryPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("Partial/_ArticleCategoryTable", categoryPage);
            }

            return View(categoryPage);
        }

        [ApplicationAuthorize(AuthorizePolicies.CanReadArticleCategory)]
        [PopulatePermissions("ArticleCategory", PolicyMethods.CanUpdate)]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var category = await _articleCategoryAppService.FindAsync(new IdRequestFilter<int>
                {
                    CanGetInactived = true,
                    Id = id
                });
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
                    StatusId = (ArticleCategoryStatuses)category.StatusId
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicies.CanCreateArticleCategory)]
        public IActionResult Create()
        {
            var model = new ArticleCategoryModel();
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanCreateArticleCategory)]
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
            var exist = await _articleCategoryAppService.FindByNameAsync(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            category.UpdatedById = LoggedUserId;
            category.CreatedById = LoggedUserId;
            var id = await _articleCategoryAppService.CreateAsync(category);

            return RedirectToAction(nameof(Detail), new { id });
        }

        [ApplicationAuthorize(AuthorizePolicies.CanUpdateArticleCategory)]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _articleCategoryAppService.FindAsync(new IdRequestFilter<int>
            {
                CanGetInactived = true,
                Id = id
            });
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
                StatusId = (ArticleCategoryStatuses)category.StatusId
            };
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateArticleCategory)]
        public async Task<IActionResult> Update(ArticleCategoryModel model)
        {
            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = await _articleCategoryAppService.FindAsync(new IdRequestFilter<int>
            {
                CanGetInactived = true,
                Id = model.Id
            });
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            var category = new ArticleCategoryModifyRequest
            {
                Description = model.Description,
                ParentId = model.ParentId,
                Name = model.Name,
                UpdatedById = LoggedUserId,
                Id = model.Id
            };

            await _articleCategoryAppService.UpdateAsync(category);
            return RedirectToAction(nameof(Detail), new { id = category.Id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateArticleCategory)]
        public async Task<IActionResult> Deactivate(ArticleCategoryIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isInactived = await _articleCategoryAppService.DeactivateAsync(new ArticleCategoryModifyRequest
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
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateArticleCategory)]
        public async Task<IActionResult> Active(ArticleCategoryIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _articleCategoryAppService.ActiveAsync(new ArticleCategoryModifyRequest
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
        [ApplicationAuthorize(AuthorizePolicies.CanDeleteArticleCategory)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _articleCategoryAppService.DeleteAsync(id);

            if (!isActived)
            {
                return RedirectToErrorPage();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanReadArticleCategory)]
        public IActionResult Search(string q, int? currentId = null, bool isParentOnly = false)
        {
            var filter = new BaseFilter
            {
                Keyword = q,
                PageSize = _pagerOptions.PageSize,
                Page = _defaultPageSelection
            };
            IList<ArticleCategoryResult> categories;
            if (isParentOnly)
            {
                categories = _articleCategoryAppService.SearchParents(new IdRequestFilter<int?>
                {
                    CanGetInactived = true,
                    Id = currentId
                }, filter);
            }
            else
            {
                categories = _articleCategoryAppService.Search(new IdRequestFilter<int?>
                {
                    CanGetInactived = true,
                    Id = currentId
                }, filter);
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
        [ApplicationAuthorize(AuthorizePolicies.CanReadArticleCategory)]
        public IActionResult SearchStatus(string q, int? currentId = null)
        {
            var statuses = _articleCategoryAppService.SearchStatus(new IdRequestFilter<int?>
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