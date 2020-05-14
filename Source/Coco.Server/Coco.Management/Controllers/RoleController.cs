using AutoMapper;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.Auth;
using Coco.Framework.Controllers;
using Coco.Management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Management.Controllers
{
    public class RoleController : BaseAuthController
    {
        private readonly IRoleBusiness _roleBusiness;
        private readonly IMapper _mapper;
        public RoleController(IMapper mapper, IRoleBusiness roleBusiness, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _roleBusiness = roleBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleBusiness.GetAsync();
            var roleModels = _mapper.Map<List<RoleViewModel>>(roles);
            var rolePage = new PagerViewModel<RoleViewModel>(roleModels);

            return View(rolePage);
        }

        [HttpGet]
        public IActionResult Search(string q)
        {
            var roles = _roleBusiness.Search(q);
            if (roles == null || !roles.Any())
            {
                return Json(new
                {
                    Items = new List<Select2Item>()
                });
            }

            var userModels = _mapper.Map<List<RoleViewModel>>(roles)
                .Select(x => new Select2Item
                {
                    Id = x.Id.ToString(),
                    Text = x.Name
                });

            return Json(userModels);
        }

        public IActionResult Detail(byte id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var role = _roleBusiness.Find(id);
                if (role == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = _mapper.Map<RoleViewModel>(role);
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
            var model = new RoleViewModel();

            return View(model);
        }

        [HttpGet]
        public IActionResult Update(byte id)
        {
            var role = _roleBusiness.Find(id);
            var model = _mapper.Map<RoleViewModel>(role);

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateOrUpdate(RoleViewModel model)
        {
            var role = _mapper.Map<RoleDto>(model);
            role.UpdatedById = LoggedUserId;
            if(role.Id > 0)
            {
                _roleBusiness.Update(role);
                return RedirectToAction("Detail", new { id = role.Id });
            }

            var exist = _roleBusiness.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            role.CreatedById = LoggedUserId;
            var newId = _roleBusiness.Add(role);
            return RedirectToAction("Detail", new { id = newId });
        }
    }
}
