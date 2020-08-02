using AutoMapper;
using Camino.Business.Contracts;
using Camino.Core.Utils;
using Camino.Business.Dtos.Identity;
using Camino.Framework.Controllers;
using Module.Web.AuthorizationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Camino.Framework.Models;
using Camino.Core.Enums;
using System.Threading.Tasks;
using Camino.Framework.Attributes;
using Camino.Core.Constants;

namespace Module.Web.AuthorizationManagement.Controllers
{
    public class AuthorizationPolicyController : BaseAuthController
    {
        private readonly IAuthorizationPolicyBusiness _authorizationPolicyBusiness;
        private readonly IMapper _mapper;
        public AuthorizationPolicyController(IMapper mapper, IAuthorizationPolicyBusiness authorizationPolicyBusiness, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _authorizationPolicyBusiness = authorizationPolicyBusiness;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadAuthorizationPolicy)]
        [LoadResultAuthorizations("AuthorizationPolicy", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public IActionResult Index()
        {
            var policies = _authorizationPolicyBusiness.GetFull();
            var policyModels = _mapper.Map<List<AuthorizationPolicyViewModel>>(policies);
            var policiesPage = new PagerViewModel<AuthorizationPolicyViewModel>(policyModels);

            return View(policiesPage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadAuthorizationPolicy)]
        [LoadResultAuthorizations("AuthorizationPolicy", PolicyMethod.CanUpdate)]
        public IActionResult Detail(short id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var policy = _authorizationPolicyBusiness.Find(id);
                if (policy == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = _mapper.Map<AuthorizationPolicyViewModel>(policy);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateAuthorizationPolicy)]
        public IActionResult Create()
        {
            var model = new AuthorizationPolicyViewModel()
            {
                SelectPermissionMethods = EnumUtil.ToSelectListItems<PolicyMethod>()
            };

            return View(model);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateAuthorizationPolicy)]
        public IActionResult Update(short id)
        {
            var policy = _authorizationPolicyBusiness.Find(id);
            var model = _mapper.Map<AuthorizationPolicyViewModel>(policy);

            var permissionMethod = EnumUtil.FilterEnumByName<PolicyMethod>(model.Name);
            model.SelectPermissionMethods = EnumUtil.ToSelectListItems(permissionMethod);
            model.PermissionMethod = (int)permissionMethod;
            var permissionMethodName = permissionMethod.ToString();
            model.Name = model.Name.Replace(permissionMethodName, "");

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateAuthorizationPolicy)]
        public async Task<IActionResult> Create(AuthorizationPolicyViewModel model)
        {
            if (model.Id > 0)
            {
                return RedirectToErrorPage();
            }

            var exist = await _authorizationPolicyBusiness.FindByNameAsync(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            if (model.PermissionMethod > 0)
            {
                var permissionMethod = (PolicyMethod)model.PermissionMethod;
                model.Name = $"{permissionMethod}{model.Name}";
            }

            var policy = _mapper.Map<AuthorizationPolicyDto>(model);
            policy.UpdatedById = LoggedUserId;
            policy.CreatedById = LoggedUserId;
            var newId = _authorizationPolicyBusiness.Add(policy);

            return RedirectToAction("Detail", new { id = newId });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateAuthorizationPolicy)]
        public async Task<IActionResult> Update(AuthorizationPolicyViewModel model)
        {
            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            if (model.PermissionMethod > 0)
            {
                var permissionMethod = (PolicyMethod)model.PermissionMethod;
                model.Name = $"{permissionMethod}{model.Name}";
            }

            var exist = await _authorizationPolicyBusiness.FindByNameAsync(model.Name);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            var policy = _mapper.Map<AuthorizationPolicyDto>(model);
            policy.UpdatedById = LoggedUserId;
            var newId = _authorizationPolicyBusiness.Update(policy);

            return RedirectToAction("Detail", new { id = newId });
        }
    }
}
