using Camino.Infrastructure.AspNetCore.Controllers;
using Camino.Infrastructure.AspNetCore.Models;
using Module.Web.AuthorizationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Infrastructure.Identity.Attributes;
using Camino.Shared.Enums;
using Microsoft.Extensions.Options;
using Camino.Application.Contracts.AppServices.Authorization;
using Camino.Shared.Configuration.Options;
using Camino.Infrastructure.Http.Interfaces;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Infrastructure.Identity.Core;
using Camino.Shared.Constants;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Application.Contracts;

namespace Module.Web.AuthorizationManagement.Controllers
{
    public class RoleController : BaseAuthController
    {
        private readonly IRoleAppService _roleAppService;
        private readonly IApplicationRoleManager<ApplicationRole> _roleManager;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public RoleController(IRoleAppService roleAppService, IHttpContextAccessor httpContextAccessor,
            IApplicationRoleManager<ApplicationRole> roleManager, IHttpHelper httpHelper, IOptions<PagerOptions> pagerOptions)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _roleAppService = roleAppService;
            _roleManager = roleManager;
            _pagerOptions = pagerOptions.Value;
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanReadRole)]
        [PopulatePermissions("Role", PolicyMethods.CanCreate, PolicyMethods.CanUpdate, PolicyMethods.CanDelete)]
        public async Task<IActionResult> Index(RoleFilterModel filter)
        {
            var rolePageList = await _roleAppService.GetAsync(new RoleFilter
            {
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search
            });

            var roleModels = rolePageList.Collections.Select(x => new RoleModel
            {
                CreatedById = x.CreatedById,
                CreatedByName = x.CreatedByName,
                CreatedDate = x.CreatedDate,
                UpdatedById = x.UpdatedById,
                UpdatedDate = x.UpdatedDate,
                UpdatedByName = x.UpdatedByName,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name
            });

            var rolePage = new PageListModel<RoleModel>(roleModels)
            {
                Filter = filter,
                TotalPage = rolePageList.TotalPage,
                TotalResult = rolePageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_RoleTable", rolePage);
            }

            return View(rolePage);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanReadRole)]
        public IActionResult Search(string q, List<long> currentRoleIds)
        {
            var roles = _roleAppService.Search(new BaseFilter
            {
                Keyword = q,
                PageSize = _pagerOptions.PageSize,
                Page = _defaultPageSelection
            }, currentRoleIds);
            if (roles == null || !roles.Any())
            {
                return Json(new
                {
                    Items = new List<Select2ItemModel>()
                });
            }

            var userModels = roles.Select(x => new Select2ItemModel
            {
                Id = x.Id.ToString(),
                Text = x.Name
            });

            return Json(userModels);
        }

        [ApplicationAuthorize(AuthorizePolicies.CanReadRole)]
        [PopulatePermissions("Role", PolicyMethods.CanUpdate)]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new RoleModel
                {
                    CreatedById = role.CreatedById,
                    UpdatedById = role.UpdatedById,
                    UpdatedByName = role.UpdatedByName,
                    Name = role.Name,
                    Id = role.Id,
                    Description = role.Description,
                    UpdatedDate = role.UpdatedDate,
                    CreatedByName = role.CreatedByName,
                    CreatedDate = role.CreatedDate
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanReadRole)]
        public IActionResult Create()
        {
            var model = new RoleModel();

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanCreateArticle)]
        public async Task<IActionResult> Create(RoleModel model)
        {
            if (model.Id > 0)
            {
                return RedirectToErrorPage();
            }

            var exist = await _roleAppService.FindByNameAsync(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var role = new ApplicationRole
            {
                CreatedById = LoggedUserId,
                UpdatedById = LoggedUserId,
                UpdatedByName = model.UpdatedByName,
                Name = model.Name,
                Description = model.Description,
                UpdatedDate = model.UpdatedDate,
                CreatedByName = model.CreatedByName,
                CreatedDate = model.CreatedDate
            };
            var newId = _roleManager.CreateAsync(role);
            return RedirectToAction(nameof(Detail), new { id = newId });
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateRole)]
        public async Task<IActionResult> Update(long id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            var model = new RoleModel
            {
                CreatedById = role.CreatedById,
                UpdatedById = role.UpdatedById,
                UpdatedByName = role.UpdatedByName,
                Name = role.Name,
                Id = role.Id,
                Description = role.Description,
                UpdatedDate = role.UpdatedDate,
                CreatedByName = role.CreatedByName,
                CreatedDate = role.CreatedDate
            };

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateRole)]
        public async Task<IActionResult> Update(RoleModel model)
        {
            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = await _roleAppService.FindByNameAsync(model.Name);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            var role = new ApplicationRole
            {
                UpdatedById = LoggedUserId,
                UpdatedByName = model.UpdatedByName,
                Name = model.Name,
                Id = model.Id,
                Description = model.Description,
                UpdatedDate = model.UpdatedDate,
                CreatedByName = model.CreatedByName,
                CreatedDate = model.CreatedDate
            };
            var newId = _roleManager.UpdateAsync(role);
            return RedirectToAction(nameof(Detail), new { id = newId });
        }
    }
}
