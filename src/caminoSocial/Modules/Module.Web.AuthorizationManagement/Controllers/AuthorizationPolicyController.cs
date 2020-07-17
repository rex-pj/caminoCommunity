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

        public IActionResult Index()
        {
            var policies = _authorizationPolicyBusiness.GetFull();
            var policyModels = _mapper.Map<List<AuthorizationPolicyViewModel>>(policies);
            var policiesPage = new PagerViewModel<AuthorizationPolicyViewModel>(policyModels);

            return View(policiesPage);
        }

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
        public IActionResult Create()
        {
            var model = new AuthorizationPolicyViewModel()
            {
                SelectPermissionMethods = EnumUtil.ToSelectListItems<PermissionMethod>()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Update(short id)
        {
            var policy = _authorizationPolicyBusiness.Find(id);
            var model = _mapper.Map<AuthorizationPolicyViewModel>(policy);

            var permissionMethod = EnumUtil.FilterEnumByName<PermissionMethod>(model.Name);
            model.SelectPermissionMethods = EnumUtil.ToSelectListItems(permissionMethod);
            model.PermissionMethod = (int)permissionMethod;
            var permissionMethodName = permissionMethod.ToString();
            model.Name = model.Name.Replace(permissionMethodName, "");

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateOrUpdate(AuthorizationPolicyViewModel model)
        {
            if (model.PermissionMethod > 0)
            {
                var permissionMethod = (PermissionMethod)model.PermissionMethod;
                model.Name = $"{permissionMethod}{model.Name}";
            }

            var policy = _mapper.Map<AuthorizationPolicyDto>(model);
            policy.UpdatedById = LoggedUserId;
            if(policy.Id > 0)
            {
                _authorizationPolicyBusiness.Update(policy);
                return RedirectToAction("Detail", new { id = policy.Id });
            }

            var exist = _authorizationPolicyBusiness.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            policy.CreatedById = LoggedUserId;
            var newId = _authorizationPolicyBusiness.Add(policy);

            return RedirectToAction("Detail", new { id = newId });
        }
    }
}
