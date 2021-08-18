using Camino.Core.Constants;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Core.Contracts.Helpers;
using Camino.Framework.Models;
using Camino.Core.Contracts.Services.Farms;
using Camino.Shared.Results.Farms;
using Camino.Shared.Requests.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.FarmManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.Enums;
using Camino.Shared.Requests.Farms;
using Camino.Shared.Configurations;
using Microsoft.Extensions.Options;

namespace Module.Web.FarmManagement.Controllers
{
    public class FarmTypeController : BaseAuthController
    {
        private readonly IFarmTypeService _farmTypeService;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public FarmTypeController(IFarmTypeService farmTypeService, IHttpContextAccessor httpContextAccessor, IHttpHelper httpHelper,
            IOptions<PagerOptions> pagerOptions)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _farmTypeService = farmTypeService;
            _pagerOptions = pagerOptions.Value;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadFarmType)]
        [LoadResultAuthorizations("FarmType", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(FarmTypeFilterModel filter)
        {
            var farmTypePageList = await _farmTypeService.GetAsync(new FarmTypeFilter
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search,
                UpdatedById = filter.UpdatedById,
                StatusId = filter.StatusId
            });
            var farmTypes = farmTypePageList.Collections.Select(x => new FarmTypeModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                CreatedById = x.CreatedById,
                UpdateById = x.UpdatedById,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                StatusId = (FarmTypeStatus)x.StatusId
            });

            var farmTypePage = new PageListModel<FarmTypeModel>(farmTypes)
            {
                Filter = filter,
                TotalPage = farmTypePageList.TotalPage,
                TotalResult = farmTypePageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("Partial/_FarmTypeTable", farmTypePage);
            }

            return View(farmTypePage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadFarmType)]
        [LoadResultAuthorizations("FarmType", PolicyMethod.CanUpdate)]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var farmType = await _farmTypeService.FindAsync(id);
                if (farmType == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new FarmTypeModel()
                {
                    Id = farmType.Id,
                    Name = farmType.Name,
                    Description = farmType.Description,
                    CreatedDate = farmType.CreatedDate,
                    CreatedBy = farmType.CreatedBy,
                    CreatedById = farmType.CreatedById,
                    UpdateById = farmType.UpdatedById,
                    UpdatedBy = farmType.UpdatedBy,
                    UpdatedDate = farmType.UpdatedDate,
                    StatusId = (FarmTypeStatus)farmType.StatusId
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateFarmType)]
        public IActionResult Create()
        {
            var model = new FarmTypeModel();
            return View(model);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateFarmType)]
        public async Task<IActionResult> Update(int id)
        {
            var farmType = await _farmTypeService.FindAsync(id);
            var model = new FarmTypeModel()
            {
                Id = farmType.Id,
                CreatedBy = farmType.CreatedBy,
                CreatedById = farmType.CreatedById,
                CreatedDate = farmType.CreatedDate,
                Description = farmType.Description,
                Name = farmType.Name,
                UpdateById = farmType.UpdatedById,
                UpdatedBy = farmType.UpdatedBy,
                UpdatedDate = farmType.UpdatedDate,
                StatusId = (FarmTypeStatus)farmType.StatusId
            };
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateFarmType)]
        public async Task<IActionResult> Deactivate(FarmTypeIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isInactived = await _farmTypeService.DeactivateAsync(new FarmTypeModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

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
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateFarmType)]
        public async Task<IActionResult> Active(FarmTypeIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _farmTypeService.ActiveAsync(new FarmTypeModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

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
        [ApplicationAuthorize(AuthorizePolicyConst.CanDeleteFarmType)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _farmTypeService.DeleteAsync(id);

            if (!isActived)
            {
                return RedirectToErrorPage();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateFarmType)]
        public async Task<IActionResult> Create(FarmTypeModel model)
        {
            var exist = _farmTypeService.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var farmType = new FarmTypeModifyRequest()
            {
                Description = model.Description,
                Name = model.Name,
                UpdatedById = LoggedUserId,
                CreatedById = LoggedUserId
            };

            var id = await _farmTypeService.CreateAsync(farmType);

            return RedirectToAction(nameof(Detail), new { id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateFarmType)]
        public async Task<IActionResult> Update(FarmTypeModel model)
        {
            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = await _farmTypeService.FindAsync(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            var farmType = new FarmTypeModifyRequest
            {
                Description = model.Description,
                Name = model.Name,
                UpdatedById = LoggedUserId,
                Id = model.Id
            };
            await _farmTypeService.UpdateAsync(farmType);
            return RedirectToAction(nameof(Detail), new { id = farmType.Id });
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadFarmType)]
        public async Task<IActionResult> Search(string q)
        {
            var farmTypes = await _farmTypeService.SearchAsync(new BaseFilter
            {
                Keyword = q,
                Page = _defaultPageSelection,
                PageSize = _pagerOptions.PageSize
            });
            if (farmTypes == null || !farmTypes.Any())
            {
                return Json(new List<Select2ItemModel>());
            }

            var farmTypesSeletions = farmTypes
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Name
                });

            return Json(farmTypesSeletions);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadFarmType)]
        public IActionResult SearchStatus(string q, int? currentId = null)
        {
            var statuses = _farmTypeService.SearchStatus(new IdRequestFilter<int?>
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