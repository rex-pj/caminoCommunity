using Camino.Framework.Controllers;
using Camino.Framework.Models;
using Module.Web.AuthorizationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Domain.Identities;
using Camino.Core.Constants;
using Camino.Framework.Attributes;
using Camino.Shared.Enums;
using Camino.Shared.Requests.Filters;
using Camino.Core.Contracts.Helpers;
using Camino.Core.Contracts.Services.Authorization;
using Camino.Core.Contracts.IdentityManager;
using Camino.Shared.Configurations;
using Microsoft.Extensions.Options;
using Camino.Infrastructure.Commons.Constants;

namespace Module.Web.AuthorizationManagement.Controllers
{
    public class RoleController : BaseAuthController
    {
        private readonly IRoleService _roleService;
        private readonly IApplicationRoleManager<ApplicationRole> _roleManager;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public RoleController(IRoleService roleService, IHttpContextAccessor httpContextAccessor,
            IApplicationRoleManager<ApplicationRole> roleManager, IHttpHelper httpHelper, IOptions<PagerOptions> pagerOptions)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _roleService = roleService;
            _roleManager = roleManager;
            _pagerOptions = pagerOptions.Value;
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadRole)]
        [LoadResultAuthorizations("Role", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(RoleFilterModel filter)
        {
            var rolePageList = await _roleService.GetAsync(new RoleFilter
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
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadRole)]
        public IActionResult Search(string q, List<long> currentRoleIds)
        {
            var roles = _roleService.Search(new BaseFilter
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

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadRole)]
        [LoadResultAuthorizations("Role", PolicyMethod.CanUpdate)]
        public async Task<IActionResult> Detail(byte id)
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
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadRole)]
        public IActionResult Create()
        {
            var model = new RoleModel();

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateArticle)]
        public IActionResult Create(RoleModel model)
        {
            if (model.Id > 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _roleService.FindByName(model.Name);
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
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateRole)]
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
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateRole)]
        public IActionResult Update(RoleModel model)
        {
            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _roleService.FindByName(model.Name);
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
