using AutoMapper;
using Camino.Business.Contracts;
using Camino.Framework.Controllers;
using Module.Web.AuthorizationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Camino.Framework.Attributes;
using Camino.Core.Constants;
using Camino.Core.Enums;
using Camino.Business.Dtos.General;
using Camino.Framework.Helpers.Contracts;

namespace Module.Web.AuthorizationManagement.Controllers
{
    public class RoleAuthorizationPolicyController : BaseAuthController
    {
        private readonly IRoleAuthorizationPolicyBusiness _roleAuthorizationPolicyBusiness;
        private readonly IMapper _mapper;
        private readonly IHttpHelper _httpHelper;
        public RoleAuthorizationPolicyController(IHttpContextAccessor httpContextAccessor, IRoleAuthorizationPolicyBusiness roleAuthorizationPolicyBusiness,
            IMapper mapper, IHttpHelper httpHelper) : base(httpContextAccessor)
        {
            _roleAuthorizationPolicyBusiness = roleAuthorizationPolicyBusiness;
            _httpHelper = httpHelper;
            _mapper = mapper;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadRoleAuthorizationPolicy)]
        [LoadResultAuthorizations("RoleAuthorizationPolicy", PolicyMethod.CanCreate, PolicyMethod.CanDelete)]
        public IActionResult Index(RoleAuthorizationPolicyFilterModel filter)
        {
            var filterDto = _mapper.Map<RoleAuthorizationPolicyFilterDto>(filter);
            var authorizationRoles = _roleAuthorizationPolicyBusiness.GetAuthoricationPolicyRoles(filter.Id, filterDto);

            var authorizationRolesPage = _mapper.Map<AuthorizationPolicyRolesModel>(authorizationRoles);
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
            var isSucceed = _roleAuthorizationPolicyBusiness.Add(model.RoleId, model.AuthorizationPolicyId, LoggedUserId);
            if (isSucceed)
            {
                return RedirectToAction("Index", new { id = model.AuthorizationPolicyId });
            }
            return RedirectToAction("Index", new { id = model.AuthorizationPolicyId });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanDeleteUserAuthorizationPolicy)]
        public IActionResult Ungrant(long roleId, short authorizationPolicyId)
        {
            var isSucceed = _roleAuthorizationPolicyBusiness.Delete(roleId, authorizationPolicyId);
            if (isSucceed)
            {
                return RedirectToAction("Index", new { id = authorizationPolicyId });
            }
            return RedirectToAction("Index", new { id = authorizationPolicyId });
        }
    }
}