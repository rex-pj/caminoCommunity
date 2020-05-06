using AutoMapper;
using Coco.Business.Contracts;
using Coco.Framework.Controllers;
using Coco.Management.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Management.Controllers
{
    public class UserController : BaseAuthController
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IMapper _mapper;
        public UserController(IMapper mapper, IUserBusiness userBusiness)
        {
            _mapper = mapper;
            _userBusiness = userBusiness;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var users = _userBusiness.GetFull();
            var userModels = _mapper.Map<List<UserViewModel>>(users);
            var userPage = new PagerViewModel<UserViewModel>(userModels);

            return View(userPage);
        }
    }
}