using AutoMapper;
using Camino.Service.Data.Filters;
using Camino.Framework.Controllers;
using Camino.Framework.Models;
using Module.Web.AuthenticationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Camino.Framework.Attributes;
using Camino.Core.Constants;
using Camino.Core.Enums;
using System.Threading.Tasks;
using Camino.Framework.Helpers.Contracts;
using Camino.Service.Business.Users.Contracts;

namespace Module.Web.AuthenticationManagement.Controllers
{
    public class UserController : BaseAuthController
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IMapper _mapper;
        private readonly IHttpHelper _httpHelper;

        public UserController(IMapper mapper, IUserBusiness userBusiness, IHttpContextAccessor httpContextAccessor,
            IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _httpHelper = httpHelper;
            _userBusiness = userBusiness;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUser)]
        [LoadResultAuthorizations("User", PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(UserFilterModel filter)
        {
            var filterDto = _mapper.Map<UserFilter>(filter);
            var userPageList = await _userBusiness.GetAsync(filterDto);
            var users = _mapper.Map<List<UserModel>>(userPageList.Collections);
            var userPage = new PageListModel<UserModel>(users) { 
                Filter = filter,
                TotalPage = userPageList.TotalPage,
                TotalResult = userPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_UserTable", userPage);
            }

            return View(userPage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUser)]
        public async Task<IActionResult> Detail(long id)
        {
            var userDto = await _userBusiness.FindFullByIdAsync(id);
            var user = _mapper.Map<UserModel>(userDto);
            
            return View(user);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUser)]
        public IActionResult Search(string q, List<long> currentUserIds)
        {
            var users = _userBusiness.Search(q, currentUserIds);
            if (users == null || !users.Any())
            {
                return Json(new
                {
                    Items = new List<Select2ItemModel>()
                });
            }

            var userModels = users.Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Lastname + " " + x.Firstname
                });

            return Json(userModels);
        }
    }
}