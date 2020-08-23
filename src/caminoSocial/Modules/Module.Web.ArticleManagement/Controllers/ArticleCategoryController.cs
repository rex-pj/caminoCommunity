using AutoMapper;
using Camino.Service.Data.Content;
using Camino.Service.Data.Filters;
using Camino.Core.Constants;
using Camino.Core.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Helpers.Contracts;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Module.Web.ArticleManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Articles.Contracts;

namespace Module.Web.ArticleManagement.Controllers
{
    public class ArticleCategoryController : BaseAuthController
    {
        private readonly IArticleCategoryBusiness _articleCategoryBusiness;
        private readonly IMapper _mapper;
        private readonly IHttpHelper _httpHelper;

        public ArticleCategoryController(IMapper mapper, IArticleCategoryBusiness articleCategoryBusiness,
            IHttpContextAccessor httpContextAccessor, IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _mapper = mapper;
            _articleCategoryBusiness = articleCategoryBusiness;
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

            var categoryPageList = await _articleCategoryBusiness.GetAsync(filterRequest);
            var categories = _mapper.Map<List<ArticleCategoryModel>>(categoryPageList.Collections);
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
        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var category = _articleCategoryBusiness.Find(id);
                if (category == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = _mapper.Map<ArticleCategoryModel>(category);
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

        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticleCategory)]
        public IActionResult Update(int id)
        {
            var category = _articleCategoryBusiness.Find(id);
            var model = _mapper.Map<ArticleCategoryModel>(category);
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateArticleCategory)]
        public IActionResult Create(ArticleCategoryModel model)
        {
            var category = _mapper.Map<ArticleCategoryProjection>(model);
            var exist = _articleCategoryBusiness.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            category.UpdatedById = LoggedUserId;
            category.CreatedById = LoggedUserId;
            var id = _articleCategoryBusiness.Add(category);

            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticleCategory)]
        public IActionResult Update(ArticleCategoryModel model)
        {
            var category = _mapper.Map<ArticleCategoryProjection>(model);
            if (category.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _articleCategoryBusiness.Find(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            category.UpdatedById = LoggedUserId;
            _articleCategoryBusiness.Update(category);
            return RedirectToAction("Detail", new { id = category.Id });
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadArticleCategory)]
        public IActionResult Search(string q, long? currentId = null, bool isParentOnly = false)
        {
            IList<ArticleCategoryProjection> categories;
            if (isParentOnly)
            {
                categories = _articleCategoryBusiness.SearchParents(q, currentId);
            }
            else
            {
                categories = _articleCategoryBusiness.Search(q, currentId);
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