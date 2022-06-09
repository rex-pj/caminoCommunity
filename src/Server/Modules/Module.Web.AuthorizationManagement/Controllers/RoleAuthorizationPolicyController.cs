using Camino.Framework.Controllers;
using Module.Web.AuthorizationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Camino.Framework.Attributes;
using Camino.Shared.Enums;
using System.Linq;
using Microsoft.Extensions.Options;
using Camino.Application.Contracts.AppServices.Authorization;
using Camino.Shared.Constants;
using Camino.Shared.Configuration.Options;
using Camino.Infrastructure.Http.Interfaces;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using System.Threading.Tasks;

namespace Module.Web.AuthorizationManagement.Controllers
{
    public class RoleAuthorizationPolicyController : BaseAuthController
    {
        private readonly IRoleAuthorizationPolicyAppService _roleAuthorizationPolicyAppService;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;

        public RoleAuthorizationPolicyController(IHttpContextAccessor httpContextAccessor,
            IRoleAuthorizationPolicyAppService roleAuthorizationPolicyAppService, IHttpHelper httpHelper,
            IOptions<PagerOptions> pagerOptions) : base(httpContextAccessor)
        {
            _roleAuthorizationPolicyAppService = roleAuthorizationPolicyAppService;
            _httpHelper = httpHelper;
            _pagerOptions = pagerOptions.Value;
        }

        [ApplicationAuthorize(AuthorizePolicies.CanReadRoleAuthorizationPolicy)]
        [LoadResultAuthorizations("RoleAuthorizationPolicy", PolicyMethods.CanCreate, PolicyMethods.CanDelete)]
        public async Task<IActionResult> Index(RoleAuthorizationPolicyFilterModel filter)
        {
            var filterRequest = new RoleAuthorizationPolicyFilter
            {
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search
            };
            var authorizationRole = await _roleAuthorizationPolicyAppService.GetPageListAsync(filter.Id, filterRequest);
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
        [ApplicationAuthorize(AuthorizePolicies.CanCreateRoleAuthorizationPolicy)]
        public async Task<IActionResult> Grant(RoleAuthorizationPolicyModel model)
        {
            var isSucceed = await _roleAuthorizationPolicyAppService.CreateAsync(model.RoleId, model.AuthorizationPolicyId, LoggedUserId);
            if (isSucceed)
            {
                return RedirectToAction(nameof(Index), new { id = model.AuthorizationPolicyId });
            }
            return RedirectToAction(nameof(Index), new { id = model.AuthorizationPolicyId });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanDeleteUserAuthorizationPolicy)]
        public async Task<IActionResult> Ungrant(long roleId, short authorizationPolicyId)
        {
            var isSucceed = await _roleAuthorizationPolicyAppService.DeleteAsync(roleId, authorizationPolicyId);
            if (isSucceed)
            {
                return RedirectToAction(nameof(Index), new { id = authorizationPolicyId });
            }
            return RedirectToAction(nameof(Index), new { id = authorizationPolicyId });
        }
    }
}