using Camino.Core.Utils;
using Camino.Framework.Controllers;
using Module.Web.AuthorizationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Camino.Framework.Models;
using Camino.Shared.Enums;
using System.Threading.Tasks;
using Camino.Framework.Attributes;
using Camino.Core.Constants;
using Camino.Core.Domain.Identities;
using Camino.Shared.Requests.Filters;
using Camino.Core.Contracts.Helpers;
using Camino.Core.Contracts.IdentityManager;
using Camino.Core.Contracts.Services.Authorization;
using Camino.Shared.Requests.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Module.Web.AuthorizationManagement.Controllers
{
    public class AuthorizationPolicyController : BaseAuthController
    {
        private readonly IAuthorizationPolicyService _authorizationPolicyService;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IHttpHelper _httpHelper;

        public AuthorizationPolicyController(IAuthorizationPolicyService authorizationPolicyService, IHttpContextAccessor httpContextAccessor,
            IUserManager<ApplicationUser> userManager, IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _userManager = userManager;
            _httpHelper = httpHelper;
            _authorizationPolicyService = authorizationPolicyService;
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadAuthorizationPolicy)]
        [LoadResultAuthorizations("AuthorizationPolicy", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(AuthorizationPolicyFilterModel filter)
        {
            var filterRequest = new AuthorizationPolicyFilter
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search
            };
            var policiesPageList = _authorizationPolicyService.Get(filterRequest);

            var policyModels = policiesPageList.Collections.Select(x => new AuthorizationPolicyModel
            {
                CreatedById = x.CreatedById,
                CreatedDate = x.CreatedDate,
                CreatedByName = x.CreatedByName,
                UpdatedById = x.UpdatedById,
                UpdatedDate = x.CreatedDate,
                UpdatedByName = x.CreatedByName,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name
            }).ToList();
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
                var policy = _authorizationPolicyService.Find(id);
                if (policy == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new AuthorizationPolicyModel
                {
                    CreatedById = policy.CreatedById,
                    CreatedDate = policy.CreatedDate,
                    CreatedByName = policy.CreatedByName,
                    UpdatedById = policy.UpdatedById,
                    UpdatedDate = policy.CreatedDate,
                    UpdatedByName = policy.CreatedByName,
                    Description = policy.Description,
                    Id = policy.Id,
                    Name = policy.Name
                };
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
                SelectPermissionMethods = EnumUtil.ToSelectOptions<PolicyMethod>().Select(x => new SelectListItem
                {
                    Text = x.Text,
                    Value = x.Id
                })
            };

            return View(model);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateAuthorizationPolicy)]
        public IActionResult Update(short id)
        {
            var policy = _authorizationPolicyService.Find(id);
            var model = new AuthorizationPolicyModel
            {
                CreatedById = policy.CreatedById,
                CreatedDate = policy.CreatedDate,
                CreatedByName = policy.CreatedByName,
                UpdatedById = policy.UpdatedById,
                UpdatedDate = policy.CreatedDate,
                UpdatedByName = policy.CreatedByName,
                Description = policy.Description,
                Id = policy.Id,
                Name = policy.Name
            };

            var permissionMethod = EnumUtil.FilterEnumByName<PolicyMethod>(model.Name);
            model.SelectPermissionMethods = EnumUtil.ToSelectOptions(permissionMethod).Select(x => new SelectListItem
            {
                Selected = x.IsSelected,
                Value = x.Id,
                Text = x.Text
            });
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

            var exist = await _authorizationPolicyService.FindByNameAsync(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            if (model.PermissionMethod > 0)
            {
                var permissionMethod = (PolicyMethod)model.PermissionMethod;
                model.Name = $"{permissionMethod}{model.Name}";
            }

            var policy = new AuthorizationPolicyRequest
            {
                CreatedById = LoggedUserId,
                CreatedDate = model.CreatedDate,
                CreatedByName = model.CreatedByName,
                UpdatedById = LoggedUserId,
                UpdatedDate = model.CreatedDate,
                UpdatedByName = model.CreatedByName,
                Description = model.Description,
                Name = model.Name
            };

            var newId = _authorizationPolicyService.Create(policy);
            return RedirectToAction(nameof(Detail), new { id = newId });
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

            var exist = await _authorizationPolicyService.FindByNameAsync(model.Name);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            var policy = new AuthorizationPolicyRequest
            {
                CreatedDate = model.CreatedDate,
                CreatedByName = model.CreatedByName,
                UpdatedById = LoggedUserId,
                UpdatedDate = model.CreatedDate,
                UpdatedByName = model.CreatedByName,
                Description = model.Description,
                Id = model.Id,
                Name = model.Name
            };
            policy.UpdatedById = LoggedUserId;
            await _authorizationPolicyService.UpdateAsync(policy);

            return RedirectToAction(nameof(Detail), new { id = model.Id });
        }
    }
}
