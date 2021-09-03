using Camino.Framework.Controllers;
using Module.Web.AuthorizationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Camino.Framework.Attributes;
using Camino.Shared.Enums;
using Camino.Shared.Requests.Filters;
using Camino.Core.Contracts.Helpers;
using Camino.Core.Contracts.Services.Authorization;
using System.Linq;
using Camino.Shared.Configurations;
using Microsoft.Extensions.Options;
using Camino.Infrastructure.Commons.Constants;

namespace Module.Web.AuthorizationManagement.Controllers
{
    public class UserAuthorizationPolicyController : BaseAuthController
    {
        private readonly IUserAuthorizationPolicyService _userAuthorizationPolicyService;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;

        public UserAuthorizationPolicyController(IHttpContextAccessor httpContextAccessor,
            IUserAuthorizationPolicyService userAuthorizationPolicyService, IHttpHelper httpHelper,
            IOptions<PagerOptions> pagerOptions) : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _userAuthorizationPolicyService = userAuthorizationPolicyService;
            _pagerOptions = pagerOptions.Value;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUserAuthorizationPolicy)]
        [LoadResultAuthorizations("UserAuthorizationPolicy", PolicyMethod.CanCreate, PolicyMethod.CanDelete)]
        public IActionResult Index(UserAuthorizationPolicyFilterModel filter)
        {
            var filterRequest = new UserAuthorizationPolicyFilter
            {
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search
            };
            var authorizationPolicy = _userAuthorizationPolicyService.GetAuthoricationPolicyUsers(filter.Id, filterRequest);

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
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateUserAuthorizationPolicy)]
        public IActionResult Grant(UserAuthorizationPolicyModel model)
        {
            var isSucceed = _userAuthorizationPolicyService.Create(model.UserId, model.AuthorizationPolicyId, LoggedUserId);
            if (isSucceed)
            {
                return RedirectToAction(nameof(Index), new { id = model.AuthorizationPolicyId });
            }
            return RedirectToAction(nameof(Index), new { id = model.AuthorizationPolicyId });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanDeleteUserAuthorizationPolicy)]
        public IActionResult Ungrant(long userId, short authorizationPolicyId)
        {
            var isSucceed = _userAuthorizationPolicyService.Delete(userId, authorizationPolicyId);
            if (isSucceed)
            {
                return RedirectToAction(nameof(Index), new { id = authorizationPolicyId });
            }
            return RedirectToAction(nameof(Index), new { id = authorizationPolicyId });
        }
    }
}