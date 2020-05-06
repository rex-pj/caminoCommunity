using AutoMapper;
using Coco.Business.Contracts;
using Coco.Entities.Domain.Auth;
using Coco.Framework.Controllers;
using Coco.Management.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Management.Controllers
{
    public class RoleController : BaseAuthController
    {
        private readonly IRoleBusiness _roleBusiness;
        private readonly IMapper _mapper;
        public RoleController(IMapper mapper, IRoleBusiness roleBusiness)
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
    }
}
