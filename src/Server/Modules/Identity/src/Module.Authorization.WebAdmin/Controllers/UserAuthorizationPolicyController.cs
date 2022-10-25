using Camino.Infrastructure.AspNetCore.Controllers;
using Module.Authorization.WebAdmin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Camino.Infrastructure.Identity.Attributes;
using Camino.Shared.Enums;
using System.Linq;
using Microsoft.Extensions.Options;
using Camino.Application.Contracts.AppServices.Authorization;
using Camino.Infrastructure.Http.Interfaces;
using Camino.Shared.Configuration.Options;
using Camino.Shared.Constants;
using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using System.Threading.Tasks;

namespace Module.Authorization.WebAdmin.Controllers
{
    public class UserAuthorizationPolicyController : BaseAuthController
    {
        private readonly IUserAuthorizationPolicyAppService _userAuthorizationPolicyAppService;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;

        public UserAuthorizationPolicyController(IHttpContextAccessor httpContextAccessor,
            IUserAuthorizationPolicyAppService userAuthorizationPolicyAppService, IHttpHelper httpHelper,
            IOptions<PagerOptions> pagerOptions) : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _userAuthorizationPolicyAppService = userAuthorizationPolicyAppService;
            _pagerOptions = pagerOptions.Value;
        }

        [ApplicationAuthorize(AuthorizePolicies.CanReadUserAuthorizationPolicy)]
        [PopulatePermissions("UserAuthorizationPolicy", PolicyMethods.CanCreate, PolicyMethods.CanDelete)]
        public IActionResult Index(UserAuthorizationPolicyFilterModel filter)
        {
            var filterRequest = new UserAuthorizationPolicyFilter
            {
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search
            };
            var authorizationPolicy = _userAuthorizationPolicyAppService.GetAuthoricationPolicyUsers(filter.Id, filterRequest);

            var users = authorizationPolicy.Collections.Select(x => new UserModel
            {
                DisplayName = x.DisplayName,
                Firstname = x.Firstname,
                Lastname = x.Lastname,
                Id = x.Id
            });
            var authorizationUsersPage = new AuthorizationPolicyUsersModel(users)
            {
                Description = authorizationPolicy.Description,
                Id = authorizationPolicy.Id,
                Name = authorizationPolicy.Name,
                TotalPage = authorizationPolicy.TotalPage,
                TotalResult = authorizationPolicy.TotalResult,
                Filter = filter
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_UserAuthorizationPolicyTable", authorizationUsersPage);
            }

            return View(authorizationUsersPage);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanCreateUserAuthorizationPolicy)]
        public async Task<IActionResult> Grant(UserAuthorizationPolicyModel model)
        {
            var isSucceed = await _userAuthorizationPolicyAppService.CreateAsync(model.UserId, model.AuthorizationPolicyId, LoggedUserId);
            if (isSucceed)
            {
                return RedirectToAction(nameof(Index), new { id = model.AuthorizationPolicyId });
            }
            return RedirectToAction(nameof(Index), new { id = model.AuthorizationPolicyId });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanDeleteUserAuthorizationPolicy)]
        public async Task<IActionResult> Ungrant(long userId, short authorizationPolicyId)
        {
            var isSucceed = await _userAuthorizationPolicyAppService.DeleteAsync(userId, authorizationPolicyId);
            if (isSucceed)
            {
                return RedirectToAction(nameof(Index), new { id = authorizationPolicyId });
            }
            return RedirectToAction(nameof(Index), new { id = authorizationPolicyId });
        }
    }
}