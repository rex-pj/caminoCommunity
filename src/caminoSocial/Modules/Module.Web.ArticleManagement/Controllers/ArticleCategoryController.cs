﻿using AutoMapper;
using Camino.Business.Contracts;
using Camino.Business.Dtos.Content;
using Camino.Core.Constants;
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
    public class ArticleCategoryController : BaseAuthController
    {
        private readonly IArticleCategoryBusiness _articleCategoryBusiness;
        private readonly IMapper _mapper;

        public ArticleCategoryController(IMapper mapper, IArticleCategoryBusiness articleCategoryBusiness, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _articleCategoryBusiness = articleCategoryBusiness;
        }

        [ApplicationAuthorization(policy: AuthorizationPolicyConst.CanReadArticleCategory)]
        public IActionResult Index()
        {
            var categories = _articleCategoryBusiness.GetFull();
            var categoryModels = _mapper.Map<List<ArticleCategoryViewModel>>(categories);
            var categoryPage = new PagerViewModel<ArticleCategoryViewModel>(categoryModels);

            return View(categoryPage);
        }

        [ApplicationAuthorization(policy: AuthorizationPolicyConst.CanReadArticleCategory)]
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

                var model = _mapper.Map<ArticleCategoryViewModel>(category);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorization(policy: AuthorizationPolicyConst.CanCreateArticleCategory)]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new ArticleCategoryViewModel()
            {
                SelectCategories = _articleCategoryBusiness
                .Get()
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(model);
        }

        [ApplicationAuthorization(policy: AuthorizationPolicyConst.CanUpdateArticleCategory)]
        [HttpGet]
        public IActionResult Update(int id)
        {
            var category = _articleCategoryBusiness.Find(id);
            var model = _mapper.Map<ArticleCategoryViewModel>(category);

            if (category.ParentId.HasValue)
            {
                model.SelectCategories = _articleCategoryBusiness
                .Get(x => x.Id != id && !x.ParentId.HasValue)
                .Where(x => x.Id != id)
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            }

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorization(policy: AuthorizationPolicyConst.CanCreateArticleCategory)]
        public IActionResult Create(ArticleCategoryViewModel model)
        {
            var category = _mapper.Map<ArticleCategoryDto>(model);
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
        [ApplicationAuthorization(policy: AuthorizationPolicyConst.CanUpdateArticleCategory)]
        public IActionResult Update(ArticleCategoryViewModel model)
        {
            var category = _mapper.Map<ArticleCategoryDto>(model);
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
    }
}