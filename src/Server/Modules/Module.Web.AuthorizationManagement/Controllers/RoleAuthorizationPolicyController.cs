using Camino.Framework.Controllers;
using Module.Web.AuthorizationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Camino.Framework.Attributes;
using Camino.Core.Constants;
using Camino.Shared.Enums;
using Camino.Shared.Requests.Filters;
using Camino.Core.Contracts.Helpers;
using Camino.Core.Contracts.Services.Authorization;
using System.Linq;

namespace Module.Web.AuthorizationManagement.Controllers
{
    public class RoleAuthorizationPolicyController : BaseAuthController
    {
        private readonly IRoleAuthorizationPolicyService _roleAuthorizationPolicyService;
        private readonly IHttpHelper _httpHelper;
        public RoleAuthorizationPolicyController(IHttpContextAccessor httpContextAccessor,
            IRoleAuthorizationPolicyService roleAuthorizationPolicyService,
            IHttpHelper httpHelper) : base(httpContextAccessor)
        {
            _roleAuthorizationPolicyService = roleAuthorizationPolicyService;
            _httpHelper = httpHelper;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadRoleAuthorizationPolicy)]
        [LoadResultAuthorizations("RoleAuthorizationPolicy", PolicyMethod.CanCreate, PolicyMethod.CanDelete)]
        public IActionResult Index(RoleAuthorizationPolicyFilterModel filter)
        {
            var filterRequest = new RoleAuthorizationPolicyFilter
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search
            };
            var authorizationRole = _roleAuthorizationPolicyService.GetAuthoricationPolicyRoles(filter.Id, filterRequest);
            var roles = authorizationRole.Collections.Select(x => new RoleModel
            {
                CreatedById = x.CreatedById,
                Name = x.Name,
                CreatedByName = x.CreatedByName,
                Description = x.Description,
                CreatedDate = x.CreatedDate,
                Id = x.Id,
                UpdatedById = x.UpdatedById,
                UpdatedByName = x.UpdatedByName,
                UpdatedDate = x.UpdatedDate
            });
            var authorizationRolesPage = new AuthorizationPolicyRolesModel(roles)
            {
                Description = authorizationRole.Description,
                Filter = filter,
                Id = authorizationRole.Id,
                Name = authorizationRole.Name,
                TotalPage = authorizationRole.TotalPage,
                TotalResult = authorizationRole.TotalResult,
            };
            authorizationRolesPage.Filter = filter;

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_RoleAuthorizationPolicyTable", authorizationRolesPage);
            }

            return View(authorizationRolesPage);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateRoleAuthorizationPolicy)]
        public IActionResult Grant(RoleAuthorizationPolicyModel model)
        {
            var isSucceed = _roleAuthorizationPolicyService.Create(model.RoleId, model.AuthorizationPolicyId, LoggedUserId);
            if (isSucceed)
            {
                return RedirectToAction(nameof(Index), new { id = model.AuthorizationPolicyId });
            }
            return RedirectToAction(nameof(Index), new { id = model.AuthorizationPolicyId });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanDeleteUserAuthorizationPolicy)]
        public IActionResult Ungrant(long roleId, short authorizationPolicyId)
        {
            var isSucceed = _roleAuthorizationPolicyService.Delete(roleId, authorizationPolicyId);
            if (isSucceed)
            {
                return RedirectToAction(nameof(Index), new { id = authorizationPolicyId });
            }
            return RedirectToAction(nameof(Index), new { id = authorizationPolicyId });
        }
    }
}