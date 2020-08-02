using AutoMapper;
using Camino.Business.Contracts;
using Camino.Business.Dtos.Content;
using Camino.Core.Constants;
using Camino.Core.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Module.Web.ArticleManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Web.ArticleManagement.Controllers
{
    public class ArticleController : BaseAuthController
    {
        private readonly IArticleBusiness _articleBusiness;
        private readonly IArticleCategoryBusiness _articleCategoryBusiness;
        private readonly IMapper _mapper;

        public ArticleController(IMapper mapper, IArticleBusiness articleBusiness,
            IArticleCategoryBusiness articleCategoryBusiness, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _articleBusiness = articleBusiness;
            _articleCategoryBusiness = articleCategoryBusiness;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadArticle)]
        [LoadResultAuthorizations("Article", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public IActionResult Index()
        {
            var articles = _articleBusiness.GetFull();
            var articleModels = _mapper.Map<List<ArticleViewModel>>(articles);
            var articlePage = new PagerViewModel<ArticleViewModel>(articleModels);

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

                var model = _mapper.Map<ArticleViewModel>(article);
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
            var model = new ArticleViewModel()
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
            var model = _mapper.Map<ArticleViewModel>(article);

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
        public IActionResult Create(ArticleViewModel model)
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
        public IActionResult Update(ArticleViewModel model)
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