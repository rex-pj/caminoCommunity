﻿using Camino.Core.Constants;
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

namespace Module.Web.FarmManagement.Controllers
{
    public class FarmTypeController : BaseAuthController
    {
        private readonly IFarmTypeService _farmTypeService;
        private readonly IHttpHelper _httpHelper;

        public FarmTypeController(IFarmTypeService farmTypeService, IHttpContextAccessor httpContextAccessor, IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _farmTypeService = farmTypeService;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadFarmType)]
        [LoadResultAuthorizations("FarmType", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(FarmTypeFilterModel filter)
        {
            var filterRequest = new FarmTypeFilter()
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search,
                UpdatedById = filter.UpdatedById
            };

            var farmTypePageList = await _farmTypeService.GetAsync(filterRequest);
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
                UpdatedDate = x.UpdatedDate
            });

            var farmTypePage = new PageListModel<FarmTypeModel>(farmTypes)
            {
                Filter = filter,
                TotalPage = farmTypePageList.TotalPage,
                TotalResult = farmTypePageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_FarmTypeTable", farmTypePage);
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
                    UpdatedDate = farmType.UpdatedDate
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
                UpdatedDate = farmType.UpdatedDate
            };
            return View(model);
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

            var farmType = new FarmTypeModifyRequest()
            {
                Description = model.Description,
                Name = model.Name,
                UpdatedById = LoggedUserId
            };
            await _farmTypeService.UpdateAsync(farmType);
            return RedirectToAction(nameof(Detail), new { id = farmType.Id });
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadFarmType)]
        public async Task<IActionResult> Search(string q)
        {
            var farmTypes = await _farmTypeService.SearchAsync(q);
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
    }
}