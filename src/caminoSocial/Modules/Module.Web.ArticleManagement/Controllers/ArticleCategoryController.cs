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

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadArticleCategory)]
        [LoadResultAuthorizations("ArticleCategory", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public IActionResult Index()
        {
            var categories = _articleCategoryBusiness.GetFull();
            var categoryModels = _mapper.Map<List<ArticleCategoryModel>>(categories);
            var categoryPage = new PageListModel<ArticleCategoryModel>(categoryModels);

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
            var model = new ArticleCategoryModel()
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

        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticleCategory)]
        public IActionResult Update(int id)
        {
            var category = _articleCategoryBusiness.Find(id);
            var model = _mapper.Map<ArticleCategoryModel>(category);

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
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateArticleCategory)]
        public IActionResult Create(ArticleCategoryModel model)
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
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticleCategory)]
        public IActionResult Update(ArticleCategoryModel model)
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