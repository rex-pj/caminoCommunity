using Camino.Shared.Requests.Filters;
using Camino.Core.Constants;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Core.Contracts.Helpers;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.IdentityManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Services.Users;
using Camino.Shared.Enums;
using Camino.Shared.Requests.Identifiers;
using Camino.Shared.Configurations;
using Microsoft.Extensions.Options;

namespace Module.Web.IdentityManagement.Controllers
{
    public class UserStatusController : BaseAuthController
    {
        private readonly IUserStatusService _userStatusService;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public UserStatusController(IHttpContextAccessor httpContextAccessor, IUserStatusService userStatusService,
            IHttpHelper httpHelper, IOptions<PagerOptions> pagerOptions)
            : base(httpContextAccessor)
        {
            _userStatusService = userStatusService;
            _httpHelper = httpHelper;
            _pagerOptions = pagerOptions.Value;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUserStatus)]
        [LoadResultAuthorizations("UserStatus", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(UserStatusFilterModel filter)
        {
            var filterRequest = new UserStatusFilter()
            {
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search
            };

            var statusPageList = await _userStatusService.GetAsync(filterRequest);
            var statuses = statusPageList.Collections.Select(x => new UserStatusModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            });

            var statusPage = new PageListModel<UserStatusModel>(statuses)
            {
                Filter = filter,
                TotalPage = statusPageList.TotalPage,
                TotalResult = statusPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_UserStatusTable", statusPage);
            }

            return View(statusPage);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUserStatus)]
        public IActionResult Search(string q)
        {
            var statuses = _userStatusService.Search(new BaseFilter
            {
                Keyword = q,
                PageSize = _pagerOptions.PageSize,
                Page = _defaultPageSelection
            });
            if (statuses == null || !statuses.Any())
            {
                return Json(new
                {
                    Items = new List<Select2ItemModel>()
                });
            }

            var userModels = statuses
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Name
                });

            return Json(userModels);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUserStatus)]
        [LoadResultAuthorizations("UserStatus", PolicyMethod.CanUpdate)]
        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var status = _userStatusService.Find(id);
                if (status == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new UserStatusModel()
                {
                    Id = status.Id,
                    Name = status.Name,
                    Description = status.Description
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateUserStatus)]
        public IActionResult Create()
        {
            var model = new UserStatusModel();
            return View(model);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateUserStatus)]
        public IActionResult Update(int id)
        {
            var status = _userStatusService.Find(id);
            var model = new UserStatusModel()
            {
                Id = status.Id,
                Name = status.Name,
                Description = status.Description
            };

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateUserStatus)]
        public async Task<IActionResult> Create(UserStatusModel model)
        {
            var exist = _userStatusService.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var status = new UserStatusModifyRequest()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };

            var id = await _userStatusService.CreateAsync(status);
            return RedirectToAction(nameof(Detail), new { id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateUserStatus)]
        public async Task<IActionResult> Update(UserStatusModel model)
        {
            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _userStatusService.Find(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            var status = new UserStatusModifyRequest()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };
            await _userStatusService.UpdateAsync(status);
            return RedirectToAction(nameof(Detail), new { id = status.Id });
        }
    }
}
