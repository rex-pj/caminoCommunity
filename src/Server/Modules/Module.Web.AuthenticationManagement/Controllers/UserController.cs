using Camino.Shared.Requests.Filters;
using Camino.Framework.Controllers;
using Camino.Framework.Models;
using Module.Web.AuthenticationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Camino.Framework.Attributes;
using Camino.Core.Constants;
using Camino.Shared.Enums;
using System.Threading.Tasks;
using Camino.Core.Contracts.Helpers;
using Camino.Core.Contracts.Services.Users;

namespace Module.Web.AuthenticationManagement.Controllers
{
    public class UserController : BaseAuthController
    {
        private readonly IUserService _userService;
        private readonly IHttpHelper _httpHelper;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor,
            IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _userService = userService;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUser)]
        [LoadResultAuthorizations("User", PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(UserFilterModel filter)
        {
            var filterRequest = new UserFilter
            {
                Address = filter.Address,
                BirthDateFrom = filter.BirthDateFrom,
                BirthDateTo = filter.BirthDateTo,
                CountryId = filter.CountryId,
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                GenderId = filter.GenderId,
                IsEmailConfirmed = filter.IsEmailConfirmed,
                Page = filter.Page,
                PageSize = filter.PageSize,
                PhoneNumber = filter.PhoneNumber,
                Search = filter.Search,
                StatusId = filter.StatusId,
                UpdatedById = filter.UpdatedById
            };
            var userPageList = await _userService.GetAsync(filterRequest);
            var users = userPageList.Collections.Select(x => new UserModel
            {
                Address = x.Address,
                UpdatedById = x.UpdatedById,
                StatusId = x.StatusId,
                StatusLabel = x.StatusLabel,
                BirthDate = x.BirthDate,
                CreatedById = x.CreatedById,
                CreatedDate = x.CreatedDate,
                CountryCode = x.CountryCode,
                CountryId = x.CountryId,
                CountryName = x.CountryName,
                Description = x.Description,
                DisplayName = x.DisplayName,
                Email = x.Email,
                Firstname = x.Firstname,
                Lastname = x.Lastname,
                GenderId = x.GenderId,
                GenderLabel = x.GenderLabel,
                Id = x.Id,
                IsEmailConfirmed = x.IsEmailConfirmed,
                PhoneNumber = x.PhoneNumber,
                UpdatedDate = x.UpdatedDate
            });
            var userPage = new PageListModel<UserModel>(users)
            {
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
            var userResult = await _userService.FindFullByIdAsync(id);
            var user = new UserModel
            {
                Address = userResult.Address,
                UpdatedById = userResult.UpdatedById,
                StatusId = userResult.StatusId,
                StatusLabel = userResult.StatusLabel,
                BirthDate = userResult.BirthDate,
                CreatedById = userResult.CreatedById,
                CreatedDate = userResult.CreatedDate,
                CountryCode = userResult.CountryCode,
                CountryId = userResult.CountryId,
                CountryName = userResult.CountryName,
                Description = userResult.Description,
                DisplayName = userResult.DisplayName,
                Email = userResult.Email,
                Firstname = userResult.Firstname,
                Lastname = userResult.Lastname,
                GenderId = userResult.GenderId,
                GenderLabel = userResult.GenderLabel,
                Id = userResult.Id,
                IsEmailConfirmed = userResult.IsEmailConfirmed,
                PhoneNumber = userResult.PhoneNumber,
                UpdatedDate = userResult.UpdatedDate
            };

            return View(user);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUser)]
        public IActionResult Search(string q, List<long> currentUserIds)
        {
            var users = _userService.Search(q, currentUserIds);
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