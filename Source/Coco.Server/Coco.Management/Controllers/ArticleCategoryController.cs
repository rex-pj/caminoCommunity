using AutoMapper;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.Content;
using Coco.Framework.Attributes;
using Coco.Framework.Controllers;
using Coco.Management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coco.Management.Controllers
{
    public class ArticleCategoryController : BaseAuthController
    {
        private readonly IArticleCategoryBusiness _articleCategoryBusiness;
        private readonly IMapper _mapper;
        public ArticleCategoryController(IMapper mapper, IArticleCategoryBusiness articleCategoryBusiness, IHttpContextAccessor httpContextAccessor)
            :base(httpContextAccessor)
        {
            _mapper = mapper;
            _articleCategoryBusiness = articleCategoryBusiness;
        }

        [ApplicationAuthorization(Policy = "CanReadArticleCategory")]
        public IActionResult Index()
        {
            var categories = _articleCategoryBusiness.GetFull();
            var categoryModels = _mapper.Map<List<ArticleCategoryViewModel>>(categories);
            var categoryPage = new PagerViewModel<ArticleCategoryViewModel>(categoryModels);

            return View(categoryPage);
        }

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

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ArticleCategoryViewModel()
            {
                SelectCategories = _articleCategoryBusiness
                .Get(x => !x.ParentCategoryId.HasValue)
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var category = _articleCategoryBusiness.Find(id);
            var model = _mapper.Map<ArticleCategoryViewModel>(category);

            if (category.ParentCategoryId.HasValue)
            {
                model.SelectCategories = _articleCategoryBusiness
                .Get(x => x.Id != id && !x.ParentCategoryId.HasValue)
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
        public IActionResult CreateOrUpdate(ArticleCategoryViewModel model)
        {
            var category = _mapper.Map<ArticleCategoryDto>(model);
            category.UpdatedById = LoggedUserId;
            if (category.Id > 0)
            {
                _articleCategoryBusiness.Update(category);
                return RedirectToAction("Detail", new { id = category.Id });
            }

            var exist = _articleCategoryBusiness.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            category.CreatedById = LoggedUserId;
            var newId = _articleCategoryBusiness.Add(category);

            return RedirectToAction("Detail", new { id = newId });
        }
    }
}