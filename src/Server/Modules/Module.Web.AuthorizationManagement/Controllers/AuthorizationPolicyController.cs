using Camino.Infrastructure.AspNetCore.Controllers;
using Module.Web.AuthorizationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Camino.Infrastructure.AspNetCore.Models;
using Camino.Shared.Enums;
using System.Threading.Tasks;
using Camino.Infrastructure.Identity.Attributes;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Camino.Application.Contracts.AppServices.Authorization;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Infrastructure.Http.Interfaces;
using Camino.Shared.Configuration.Options;
using Camino.Shared.Constants;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Application.Contracts.Utils;
using Camino.Shared.Utils;

namespace Module.Web.AuthorizationManagement.Controllers
{
    public class AuthorizationPolicyController : BaseAuthController
    {
        private readonly IAuthorizationPolicyAppService _authorizationPolicyAppService;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;

        public AuthorizationPolicyController(IAuthorizationPolicyAppService authorizationPolicyAppService, IHttpContextAccessor httpContextAccessor,
            IUserManager<ApplicationUser> userManager, IHttpHelper httpHelper, IOptions<PagerOptions> pagerOptions)
            : base(httpContextAccessor)
        {
            _userManager = userManager;
            _httpHelper = httpHelper;
            _authorizationPolicyAppService = authorizationPolicyAppService;
            _pagerOptions = pagerOptions.Value;
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanReadAuthorizationPolicy)]
        [PopulatePermissions("AuthorizationPolicy", PolicyMethods.CanCreate, PolicyMethods.CanUpdate, PolicyMethods.CanDelete)]
        public async Task<IActionResult> Index(AuthorizationPolicyFilterModel filter)
        {
            var filterRequest = new AuthorizationPolicyFilter
            {
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search
            };
            var policiesPageList = await _authorizationPolicyAppService.GetAsync(filterRequest);

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
            var canViewUserAuthorizationPolicy = await _userManager.HasPolicyAsync(User, AuthorizePolicies.CanReadUserAuthorizationPolicy);
            var canViewRoleAuthorizationPolicy = await _userManager.HasPolicyAsync(User, AuthorizePolicies.CanReadRoleAuthorizationPolicy);
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

        [ApplicationAuthorize(AuthorizePolicies.CanReadAuthorizationPolicy)]
        [PopulatePermissions("AuthorizationPolicy", PolicyMethods.CanUpdate)]
        public async Task<IActionResult> Detail(short id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var policy = await _authorizationPolicyAppService.FindAsync(id);
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
        [ApplicationAuthorize(AuthorizePolicies.CanCreateAuthorizationPolicy)]
        public IActionResult Create()
        {
            var model = new AuthorizationPolicyModel()
            {
                SelectPermissionMethods = SelectOptionUtils.ToSelectOptions<PolicyMethods>().Select(x => new SelectListItem
                {
                    Text = x.Text,
                    Value = x.Id
                })
            };

            return View(model);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateAuthorizationPolicy)]
        public async Task<IActionResult> Update(short id)
        {
            var policy = await _authorizationPolicyAppService.FindAsync(id);
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

            var permissionMethod = EnumUtil.FilterEnumByName<PolicyMethods>(model.Name);
            model.SelectPermissionMethods = SelectOptionUtils.ToSelectOptions(permissionMethod).Select(x => new SelectListItem
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
        [ApplicationAuthorize(AuthorizePolicies.CanCreateAuthorizationPolicy)]
        public async Task<IActionResult> Create(AuthorizationPolicyModel model)
        {
            if (model.Id > 0)
            {
                return RedirectToErrorPage();
            }

            var exist = await _authorizationPolicyAppService.FindByNameAsync(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            if (model.PermissionMethod > 0)
            {
                var permissionMethod = (PolicyMethods)model.PermissionMethod;
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

            var newId = await _authorizationPolicyAppService.CreateAsync(policy);
            return RedirectToAction(nameof(Detail), new { id = newId });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateAuthorizationPolicy)]
        public async Task<IActionResult> Update(AuthorizationPolicyModel model)
        {
            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            if (model.PermissionMethod > 0)
            {
                var permissionMethod = (PolicyMethods)model.PermissionMethod;
                model.Name = $"{permissionMethod}{model.Name}";
            }

            var exist = await _authorizationPolicyAppService.FindByNameAsync(model.Name);
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
            await _authorizationPolicyAppService.UpdateAsync(policy);

            return RedirectToAction(nameof(Detail), new { id = model.Id });
        }
    }
}
