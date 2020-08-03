using AutoMapper;
using Camino.Business.Contracts;
using Camino.Framework.Controllers;
using Module.Web.AuthorizationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Camino.Framework.Attributes;
using Camino.Core.Constants;
using Camino.Core.Enums;

namespace Module.Web.AuthorizationManagement.Controllers
{
    public class RoleAuthorizationPolicyController : BaseAuthController
    {
        private readonly IRoleAuthorizationPolicyBusiness _roleAuthorizationPolicyBusiness;
        private readonly IMapper _mapper;
        public RoleAuthorizationPolicyController(IHttpContextAccessor httpContextAccessor, IRoleAuthorizationPolicyBusiness roleAuthorizationPolicyBusiness,
            IMapper mapper) : base(httpContextAccessor)
        {
            _roleAuthorizationPolicyBusiness = roleAuthorizationPolicyBusiness;
            _mapper = mapper;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadRoleAuthorizationPolicy)]
        [LoadResultAuthorizations("RoleAuthorizationPolicy", PolicyMethod.CanCreate, PolicyMethod.CanDelete)]
        public IActionResult Index(short id)
        {
            var result = _roleAuthorizationPolicyBusiness.GetAuthoricationPolicyRoles(id);
            var authorizationRoles = _mapper.Map<AuthorizationPolicyRolesViewModel>(result);
            return View(authorizationRoles);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateRoleAuthorizationPolicy)]
        public IActionResult Grant(AuthorizationPolicyRolesViewModel model)
        {
            var isSucceed = _roleAuthorizationPolicyBusiness.Add(model.RoleId, model.Id, LoggedUserId);
            if (isSucceed)
            {
                return RedirectToAction("Index", new { id = model.Id });
            }
            return RedirectToAction("Index", new { id = model.Id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanDeleteUserAuthorizationPolicy)]
        public IActionResult Ungrant(byte roleId, short authorizationPolicyId)
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