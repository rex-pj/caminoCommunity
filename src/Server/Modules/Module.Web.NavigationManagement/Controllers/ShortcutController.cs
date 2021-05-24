using Camino.Core.Constants;
using Camino.Core.Contracts.Helpers;
using Camino.Core.Contracts.Services.Navigations;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Models;
using Camino.Infrastructure.Enums;
using Camino.Shared.Enums;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Navigations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.NavigationManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Web.NavigationManagement.Controllers
{
    public class ShortcutController : BaseAuthController
    {
        private readonly IShortcutService _shortcutService;
        private readonly IHttpHelper _httpHelper;

        public ShortcutController(IShortcutService shortcutService,
            IHttpContextAccessor httpContextAccessor, IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _shortcutService = shortcutService;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadShortcut)]
        [LoadResultAuthorizations("Shortcut", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(ShortcutFilterModel filter)
        {
            var filterRequest = new ShortcutFilter()
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search,
                TypeId = filter.TypeId
            };

            var shortcutPageList = await _shortcutService.GetAsync(filterRequest);
            var shortcuts = shortcutPageList.Collections.Select(x => new ShortcutModel
            {
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                Icon = x.Icon,
                TypeId = (ShortcutType)x.TypeId,
                Url = x.Url,
                Order = x.Order
            });
            var shortcutPage = new PageListModel<ShortcutModel>(shortcuts)
            {
                Filter = filter,
                TotalPage = shortcutPageList.TotalPage,
                TotalResult = shortcutPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_ShortcutTable", shortcutPage);
            }

            return View(shortcutPage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadShortcut)]
        [LoadResultAuthorizations("Shortcut", PolicyMethod.CanUpdate)]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var shortcut = await _shortcutService.FindAsync(id);
                if (shortcut == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new ShortcutModel
                {
                    Description = shortcut.Description,
                    Name = shortcut.Name,
                    Icon = shortcut.Icon,
                    TypeId = (ShortcutType)shortcut.TypeId,
                    Url = shortcut.Url,
                    Id = shortcut.Id,
                    Order = shortcut.Order
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateShortcut)]
        public IActionResult Create()
        {
            var model = new ShortcutModel();
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateShortcut)]
        public async Task<IActionResult> Create(ShortcutModel model)
        {
            var shortcut = new ShortcutModifyRequest
            {
                Description = model.Description,
                Name = model.Name,
                Icon = model.Icon,
                TypeId = (int)model.TypeId,
                Url = model.Url,
                Order = model.Order
            };
            var exist = await _shortcutService.FindByNameAsync(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var id = await _shortcutService.CreateAsync(shortcut);

            return RedirectToAction(nameof(Detail), new { id });
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateShortcut)]
        public async Task<IActionResult> Update(int id)
        {
            var shortcut = await _shortcutService.FindAsync(id);
            var model = new ShortcutModel
            {
                Description = shortcut.Description,
                Name = shortcut.Name,
                Icon = shortcut.Icon,
                TypeId = (ShortcutType)shortcut.TypeId,
                Url = shortcut.Url,
                Id = shortcut.Id,
                Order = shortcut.Order
            };
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateShortcut)]
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
                Order = model.Order
            };
            if (shortcut.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = await _shortcutService.FindAsync(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            await _shortcutService.UpdateAsync(shortcut);
            return RedirectToAction(nameof(Detail), new { id = shortcut.Id });
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadShortcutType)]
        public IActionResult SearchShortcutTypes(string q, int? currentId = null)
        {
            var shortcuts = _shortcutService.GetShortcutTypes(new ShortcutTypeFilter
            {
                ShortcutTypeId = currentId.HasValue ? currentId.Value : 0,
                Search = q,
                Page = 1,
                PageSize = 10
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
    }
}
