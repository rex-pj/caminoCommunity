using AutoMapper;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.Content;
using Coco.Framework.Controllers;
using Coco.Management.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Coco.Management.Controllers
{
    public class ArticleCategoryController : BaseAuthController
    {
        private readonly IArticleCategoryBusiness _articleCategoryBusiness;
        private readonly IMapper _mapper;
        public ArticleCategoryController(IMapper mapper, IArticleCategoryBusiness articleCategoryBusiness)
        {
            _mapper = mapper;
            _articleCategoryBusiness = articleCategoryBusiness;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _articleCategoryBusiness.GetAsync();
            var model = _mapper.Map<List<ArticleCategoryViewModel>>(categories);
            return View(model);
        }

        public IActionResult Detail()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var category = _articleCategoryBusiness.Find(id);
            var model = _mapper.Map<ArticleCategoryViewModel>(category);
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateOrUpdate(ArticleCategoryViewModel model)
        {
            var category = _mapper.Map<ArticleCategoryDto>(model);
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            category.UpdatedById = userId;
            if (category.Id <= 0)
            {
                category.CreatedById = userId;
                _articleCategoryBusiness.Add(category);
            }
            else
            {
                _articleCategoryBusiness.Update(category);
            }

            return RedirectToAction("Index");
        }
    }
}