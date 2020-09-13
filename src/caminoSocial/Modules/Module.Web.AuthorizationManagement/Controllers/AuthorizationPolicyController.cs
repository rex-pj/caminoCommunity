using AutoMapper;
using Camino.Core.Utils;
using Camino.Service.Projections.Identity;
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
using Camino.IdentityManager.Contracts;
using Camino.IdentityManager.Models;
using Camino.Service.Projections.Filters;
using Camino.Framework.Helpers.Contracts;
using Camino.Service.Business.Authorization.Contracts;

namespace Module.Web.AuthorizationManagement.Controllers
{
    public class AuthorizationPolicyController : BaseAuthController
    {
        private readonly IAuthorizationPolicyBusiness _authorizationPolicyBusiness;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHttpHelper _httpHelper;

        public AuthorizationPolicyController(IMapper mapper, IAuthorizationPolicyBusiness authorizationPolicyBusiness, IHttpContextAccessor httpContextAccessor,
            IUserManager<ApplicationUser> userManager, IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _userManager = userManager;
            _mapper = mapper;
            _httpHelper = httpHelper;
            _authorizationPolicyBusiness = authorizationPolicyBusiness;
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadAuthorizationPolicy)]
        [LoadResultAuthorizations("AuthorizationPolicy", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(AuthorizationPolicyFilterModel filter)
        {
            var filterRequest = _mapper.Map<AuthorizationPolicyFilter>(filter);
            var policiesPageList = _authorizationPolicyBusiness.Get(filterRequest);

            var policyModels = _mapper.Map<List<AuthorizationPolicyModel>>(policiesPageList.Collections);
            var canViewUserAuthorizationPolicy = await _userManager.HasPolicyAsync(User, AuthorizePolicyConst.CanReadUserAuthorizationPolicy);
            var canViewRoleAuthorizationPolicy = await _userManager.HasPolicyAsync(User, AuthorizePolicyConst.CanReadRoleAuthorizationPolicy);
            policyModels.ForEach(x =>
            {
                x.CanViewRoleAuthorizationPolicy = canViewRoleAuthorizationPolicy;
                x.CanViewUserAuthorizationPolicy = canViewUserAuthorizationPolicy;
            });

            var policiesPage = new PageListModel<AuthorizationPolicyModel>(policyModels)
            {
                Filter = filter,
                TotalPage = policiesPageList.TotalPage,
                TotalResult = policiesPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_AuthorizationPolicyTable", policiesPage);
            }

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

                var model = _mapper.Map<AuthorizationPolicyModel>(policy);
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
            var model = new AuthorizationPolicyModel()
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
            var model = _mapper.Map<AuthorizationPolicyModel>(policy);

            var permissionMethod = EnumUtil.FilterEnumByName<PolicyMethod>(model.Name);
            model.SelectPermissionMethods = EnumUtil.ToSelectListItems(permissionMethod);
            model.PermissionMethod = (int)permissionMethod;
            var permissionMethodName = permissionMethod.ToString();
            model.Name = model.Name.Replace(permissionMethodName, "");

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateAuthorizationPolicy)]
        public async Task<IActionResult> Create(AuthorizationPolicyModel model)
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

            var policy = _mapper.Map<AuthorizationPolicyProjection>(model);
            policy.UpdatedById = LoggedUserId;
            policy.CreatedById = LoggedUserId;
            var newId = _authorizationPolicyBusiness.Add(policy);

            return RedirectToAction("Detail", new { id = newId });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateAuthorizationPolicy)]
        public async Task<IActionResult> Update(AuthorizationPolicyModel model)
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

            var policy = _mapper.Map<AuthorizationPolicyProjection>(model);
            policy.UpdatedById = LoggedUserId;
            var newId = _authorizationPolicyBusiness.Update(policy);

            return RedirectToAction("Detail", new { id = newId });
        }
    }
}
