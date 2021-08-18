using Camino.Shared.Requests.Filters;
using Camino.Framework.Controllers;
using Camino.Framework.Models;
using Module.Web.AuthenticationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Camino.Framework.Attributes;
using Camino.Core.Constants;
using Camino.Shared.Enums;
using System.Threading.Tasks;
using Camino.Core.Contracts.Helpers;
using Camino.Core.Contracts.Services.Users;
using Camino.Shared.Requests.Identifiers;

namespace Module.Web.AuthenticationManagement.Controllers
{
    public class UserController : BaseAuthController
    {
        private readonly IUserService _userService;
        private readonly IHttpHelper _httpHelper;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor,
            IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _userService = userService;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUser)]
        [LoadResultAuthorizations("User", PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(UserFilterModel filter)
        {
            var userPageList = await _userService.GetAsync(new UserFilter
            {
                Address = filter.Address,
                BirthDateFrom = filter.BirthDateFrom,
                BirthDateTo = filter.BirthDateTo,
                CountryId = filter.CountryId,
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                GenderId = filter.GenderId,
                IsEmailConfirmed = filter.IsEmailConfirmed,
                Page = filter.Page,
                PageSize = filter.PageSize,
                PhoneNumber = filter.PhoneNumber,
                Search = filter.Search,
                StatusId = filter.StatusId,
                UpdatedById = filter.UpdatedById,
                CanGetDeleted = true,
                CanGetInactived = true
            });
            var users = userPageList.Collections.Select(x => new UserModel
            {
                Address = x.Address,
                UpdatedById = x.UpdatedById,
                StatusId = (UserStatus)x.StatusId,
                BirthDate = x.BirthDate,
                CreatedById = x.CreatedById,
                CreatedDate = x.CreatedDate,
                CountryCode = x.CountryCode,
                CountryId = x.CountryId,
                CountryName = x.CountryName,
                Description = x.Description,
                DisplayName = x.DisplayName,
                Email = x.Email,
                Firstname = x.Firstname,
                Lastname = x.Lastname,
                GenderId = x.GenderId,
                GenderLabel = x.GenderLabel,
                Id = x.Id,
                IsEmailConfirmed = x.IsEmailConfirmed,
                PhoneNumber = x.PhoneNumber,
                UpdatedDate = x.UpdatedDate
            });
            var userPage = new PageListModel<UserModel>(users)
            {
                Filter = filter,
                TotalPage = userPageList.TotalPage,
                TotalResult = userPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_UserTable", userPage);
            }

            return View(userPage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUser)]
        public async Task<IActionResult> Detail(long id)
        {
            var userResult = await _userService.FindFullByIdAsync(new IdRequestFilter<long>
            {
                Id = id,
                CanGetDeleted = true,
                CanGetInactived = true
            });
            var user = new UserModel
            {
                Address = userResult.Address,
                UpdatedById = userResult.UpdatedById,
                StatusId = (UserStatus)userResult.StatusId,
                StatusLabel = userResult.StatusLabel,
                BirthDate = userResult.BirthDate,
                CreatedById = userResult.CreatedById,
                CreatedDate = userResult.CreatedDate,
                CountryCode = userResult.CountryCode,
                CountryId = userResult.CountryId,
                CountryName = userResult.CountryName,
                Description = userResult.Description,
                DisplayName = userResult.DisplayName,
                Email = userResult.Email,
                Firstname = userResult.Firstname,
                Lastname = userResult.Lastname,
                GenderId = userResult.GenderId,
                GenderLabel = userResult.GenderLabel,
                Id = userResult.Id,
                IsEmailConfirmed = userResult.IsEmailConfirmed,
                PhoneNumber = userResult.PhoneNumber,
                UpdatedDate = userResult.UpdatedDate
            };

            return View(user);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUser)]
        public async Task<IActionResult> Search(string q, List<long> currentUserIds)
        {
            var users = await _userService.SearchAsync(new UserFilter
            {
                Search = q,
                PageSize = 10,
                Page = 1
            }, currentUserIds);
            if (users == null || !users.Any())
            {
                return Json(new
                {
                    Items = new List<Select2ItemModel>()
                });
            }

            var userModels = users.Select(x => new Select2ItemModel
            {
                Id = x.Id.ToString(),
                Text = x.Lastname + " " + x.Firstname
            });

            return Json(userModels);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateUser)]
        public async Task<IActionResult> TemporaryDelete(UserIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isDeleted = await _userService.SoftDeleteAsync(new UserModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

            if (!isDeleted)
            {
                return RedirectToErrorPage();
            }

            if (request.ShouldKeepDetailPage)
            {
                return RedirectToAction(nameof(Detail), new { id = request.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateUser)]
        public async Task<IActionResult> Deactivate(UserIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isInactived = await _userService.DeactivateAsync(new UserModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

            if (!isInactived)
            {
                return RedirectToErrorPage();
            }

            if (request.ShouldKeepDetailPage)
            {
                return RedirectToAction(nameof(Detail), new { id = request.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateUser)]
        public async Task<IActionResult> Active(UserIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _userService.ActiveAsync(new UserModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

            if (!isActived)
            {
                return RedirectToErrorPage();
            }

            if (request.ShouldKeepDetailPage)
            {
                return RedirectToAction(nameof(Detail), new { id = request.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateUser)]
        public async Task<IActionResult> Confirm(UserIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isConfirmed = await _userService.ConfirmAsync(new UserModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

            if (!isConfirmed)
            {
                return RedirectToErrorPage();
            }

            if (request.ShouldKeepDetailPage)
            {
                return RedirectToAction(nameof(Detail), new { id = request.Id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}