using AutoMapper;
using Camino.Business.Contracts;
using Camino.Business.Dtos.Content;
using Camino.Business.Dtos.General;
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

namespace Module.Web.ArticleManagement.Controllers
{
    public class ArticleController : BaseAuthController
    {
        private readonly IArticleBusiness _articleBusiness;
        private readonly IArticleCategoryBusiness _articleCategoryBusiness;
        private readonly IMapper _mapper;
        private readonly IHttpHelper _httpHelper;

        public ArticleController(IMapper mapper, IArticleBusiness articleBusiness, IHttpHelper httpHelper,
            IArticleCategoryBusiness articleCategoryBusiness, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _mapper = mapper;
            _articleBusiness = articleBusiness;
            _articleCategoryBusiness = articleCategoryBusiness;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadArticle)]
        [LoadResultAuthorizations("Article", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(ArticleFilterModel filter)
        {
            var filterDto = new ArticleFilterDto()
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search,
                UpdatedById = filter.UpdatedById
            };

            var articlePageList = await _articleBusiness.GetAsync(filterDto);
            var articles = _mapper.Map<List<ArticleModel>>(articlePageList.Collections);
            var articlePage = new PageListModel<ArticleModel>(articles)
            {
                Filter = filter,
                TotalPage = articlePageList.TotalPage,
                TotalResult = articlePageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_ArticleTable", articlePage);
            }

            return View(articlePage);

            return View(articlePage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadArticle)]
        [LoadResultAuthorizations("ArticleCategory", PolicyMethod.CanUpdate)]
        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var article = _articleBusiness.Find(id);
                if (article == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = _mapper.Map<ArticleModel>(article);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateArticle)]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new ArticleModel()
            {
                SelectCategories = _articleCategoryBusiness
                .Get()
                .Select(x => new SelectListItem()
                {
                    Text = x.ParentId.HasValue ? $"--{x.Name}" : x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(model);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticle)]
        [HttpGet]
        public IActionResult Update(int id)
        {
            var article = _articleBusiness.Find(id);
            var model = _mapper.Map<ArticleModel>(article);

            model.SelectCategories = _articleCategoryBusiness
                .Get()
                .Select(x => new SelectListItem()
                {
                    Text = x.ParentId.HasValue ? $"--{x.Name}" : x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.Id == article.ArticleCategoryId
                });

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateArticle)]
        public IActionResult Create(ArticleModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var article = _mapper.Map<ArticleDto>(model);
            var exist = _articleBusiness.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            article.UpdatedById = LoggedUserId;
            article.CreatedById = LoggedUserId;
            var id = _articleBusiness.Add(article);

            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticle)]
        public IActionResult Update(ArticleModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var article = _mapper.Map<ArticleDto>(model);
            if (article.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _articleBusiness.Find(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            article.UpdatedById = LoggedUserId;
            _articleBusiness.Update(article);
            return RedirectToAction("Detail", new { id = article.Id });
        }
    }
}