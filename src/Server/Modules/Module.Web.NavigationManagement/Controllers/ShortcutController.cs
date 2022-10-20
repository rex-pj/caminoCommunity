using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Navigations;
using Camino.Application.Contracts.AppServices.Navigations.Dtos;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Models;
using Camino.Infrastructure.Http.Interfaces;
using Camino.Shared.Configuration.Options;
using Camino.Shared.Constants;
using Camino.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Module.Web.NavigationManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Web.NavigationManagement.Controllers
{
    public class ShortcutController : BaseAuthController
    {
        private readonly IShortcutAppService _shortcutAppService;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public ShortcutController(IShortcutAppService shortcutAppService,
            IHttpContextAccessor httpContextAccessor, IHttpHelper httpHelper, IOptions<PagerOptions> pagerOptions)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _shortcutAppService = shortcutAppService;
            _pagerOptions = pagerOptions.Value;
        }

        [ApplicationAuthorize(AuthorizePolicies.CanReadShortcut)]
        [PopulatePermissions("Shortcut", PolicyMethods.CanCreate, PolicyMethods.CanUpdate, PolicyMethods.CanDelete)]
        public async Task<IActionResult> Index(ShortcutFilterModel filter)
        {
            var shortcutPageList = await _shortcutAppService.GetAsync(new ShortcutFilter
            {
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search,
                TypeId = filter.TypeId,
                CanGetInactived = true,
                StatusId = filter.StatusId
            });
            var shortcuts = shortcutPageList.Collections.Select(x => new ShortcutModel
            {
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                Icon = x.Icon,
                TypeId = (ShortcutTypes)x.TypeId,
                Url = x.Url,
                DisplayOrder = x.DisplayOrder,
                StatusId = (ShortcutStatuses)x.StatusId,
            });
            var shortcutPage = new PageListModel<ShortcutModel>(shortcuts)
            {
                Filter = filter,
                TotalPage = shortcutPageList.TotalPage,
                TotalResult = shortcutPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("Partial/_ShortcutTable", shortcutPage);
            }

            return View(shortcutPage);
        }

        [ApplicationAuthorize(AuthorizePolicies.CanReadShortcut)]
        [PopulatePermissions("Shortcut", PolicyMethods.CanUpdate)]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var shortcut = await _shortcutAppService.FindAsync(new IdRequestFilter<int>
                {
                    Id = id,
                    CanGetInactived = true
                });
                if (shortcut == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new ShortcutModel
                {
                    Description = shortcut.Description,
                    Name = shortcut.Name,
                    Icon = shortcut.Icon,
                    TypeId = (ShortcutTypes)shortcut.TypeId,
                    Url = shortcut.Url,
                    Id = shortcut.Id,
                    DisplayOrder = shortcut.DisplayOrder,
                    StatusId = (ShortcutStatuses)shortcut.StatusId
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicies.CanCreateShortcut)]
        public IActionResult Create()
        {
            var model = new ShortcutModel();
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanCreateShortcut)]
        public async Task<IActionResult> Create(ShortcutModel model)
        {
            var shortcut = new ShortcutModifyRequest
            {
                Description = model.Description,
                Name = model.Name,
                Icon = model.Icon,
                TypeId = (int)model.TypeId,
                Url = model.Url,
                Order = model.DisplayOrder,
                UpdatedById = LoggedUserId,
                CreatedById = LoggedUserId
            };
            var exist = await _shortcutAppService.FindByNameAsync(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var id = await _shortcutAppService.CreateAsync(shortcut);

            return RedirectToAction(nameof(Detail), new { id });
        }

        [ApplicationAuthorize(AuthorizePolicies.CanUpdateShortcut)]
        public async Task<IActionResult> Update(int id)
        {
            var shortcut = await _shortcutAppService.FindAsync(new IdRequestFilter<int>
            {
                Id = id,
                CanGetInactived = true
            });
            var model = new ShortcutModel
            {
                Description = shortcut.Description,
                Name = shortcut.Name,
                Icon = shortcut.Icon,
                TypeId = (ShortcutTypes)shortcut.TypeId,
                Url = shortcut.Url,
                Id = shortcut.Id,
                DisplayOrder = shortcut.DisplayOrder,
                StatusId = (ShortcutStatuses)shortcut.StatusId
            };
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateShortcut)]
        public async Task<IActionResult> Update(ShortcutModel model)
        {
            var shortcut = new ShortcutModifyRequest
            {
                Description = model.Description,
                Name = model.Name,
                Icon = model.Icon,
                TypeId = (int)model.TypeId,
                Url = model.Url,
                Id = model.Id,
                Order = model.DisplayOrder,
                UpdatedById = LoggedUserId
            };
            if (shortcut.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = await _shortcutAppService.FindAsync(new IdRequestFilter<int>
            {
                Id = model.Id,
                CanGetInactived = true
            });
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            await _shortcutAppService.UpdateAsync(shortcut);
            return RedirectToAction(nameof(Detail), new { id = shortcut.Id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateShortcut)]
        public async Task<IActionResult> Deactivate(ShortcutIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isInactived = await _shortcutAppService.DeactivateAsync(request.Id, LoggedUserId);

            if (!isInactived)
            {
                return RedirectToErrorPage();
            }

            if (request.ShouldKeepUpdatePage)
            {
                return RedirectToAction(nameof(Update), new { id = request.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateShortcut)]
        public async Task<IActionResult> Active(ShortcutIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _shortcutAppService.ActiveAsync(request.Id, LoggedUserId);

            if (!isActived)
            {
                return RedirectToErrorPage();
            }

            if (request.ShouldKeepUpdatePage)
            {
                return RedirectToAction(nameof(Update), new { id = request.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanDeleteShortcut)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _shortcutAppService.DeleteAsync(id);

            if (!isActived)
            {
                return RedirectToErrorPage();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanReadShortcutType)]
        public IActionResult SearchTypes(string q, int? currentId = null)
        {
            var shortcuts = _shortcutAppService.GetShortcutTypes(new ShortcutTypeFilter
            {
                Id = currentId,
                Keyword = q,
                Page = _defaultPageSelection,
                PageSize = _pagerOptions.PageSize
            });

            if (shortcuts == null || !shortcuts.Any())
            {
                return Json(new List<Select2ItemModel>());
            }

            var shortcutSeletions = shortcuts
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Text
                });

            return Json(shortcutSeletions);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanReadProductAttribute)]
        public IActionResult SearchStatus(string q, int? currentId = null)
        {
            var statuses = _shortcutAppService.SearchStatus(new IdRequestFilter<int?>
            {
                Id = currentId
            }, q);

            if (statuses == null || !statuses.Any())
            {
                return Json(new List<Select2ItemModel>());
            }

            var categorySeletions = statuses
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Text
                });

            return Json(categorySeletions);
        }
    }
}
