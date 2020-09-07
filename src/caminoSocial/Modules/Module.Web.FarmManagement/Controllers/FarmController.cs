using Camino.Core.Constants;
using Camino.Core.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Helpers.Contracts;
using Camino.Framework.Models;
using Camino.Service.Business.Farms.Contracts;
using Camino.Service.Data.Farm;
using Camino.Service.Data.Filters;
using Camino.Service.Data.Media;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.FarmManagement.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Web.FarmManagement.Controllers
{
    public class FarmController : BaseAuthController
    {
        private readonly IFarmBusiness _farmBusiness;
        private readonly IHttpHelper _httpHelper;

        public FarmController(IFarmBusiness farmBusiness, IHttpHelper httpHelper,
            IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _farmBusiness = farmBusiness;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadFarm)]
        [LoadResultAuthorizations("Farm", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(FarmFilterModel filter)
        {
            var filterRequest = new FarmFilter()
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search,
                UpdatedById = filter.UpdatedById,
                FarmTypeId = filter.FarmTypeId
            };

            var farmPageList = await _farmBusiness.GetAsync(filterRequest);
            var farms = farmPageList.Collections.Select(x => new FarmModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                ThumbnailId = x.ThumbnailId,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
            });
            var farmPage = new PageListModel<FarmModel>(farms)
            {
                Filter = filter,
                TotalPage = farmPageList.TotalPage,
                TotalResult = farmPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_FarmTable", farmPage);
            }

            return View(farmPage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadFarm)]
        [LoadResultAuthorizations("Farm", PolicyMethod.CanUpdate)]
        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var farm = _farmBusiness.FindDetail(id);
                if (farm == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new FarmModel()
                {
                    Description = farm.Description,
                    CreatedDate = farm.CreatedDate,
                    UpdatedDate = farm.UpdatedDate,
                    UpdatedBy = farm.UpdatedBy,
                    UpdateById = farm.UpdatedById,
                    CreatedBy = farm.CreatedBy,
                    CreatedById = farm.CreatedById,
                    FarmTypeId = farm.FarmTypeId,
                    FarmTypeName = farm.FarmTypeName,
                    Name = farm.Name,
                    Id = farm.Id,
                    ThumbnailId = farm.ThumbnailId
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateFarm)]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new FarmModel();
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateFarm)]
        public IActionResult Create(FarmModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var exist = _farmBusiness.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var farm = new FarmProjection()
            {
                UpdatedById = LoggedUserId,
                CreatedById = LoggedUserId,
                Name = model.Name,
                Description = model.Description,
                Thumbnail = new PictureLoadProjection()
                {
                    FileName = model.ThumbnailFileName,
                    ContentType = model.ThumbnailFileType,
                    Base64Data = model.Thumbnail
                },
                FarmTypeId = model.FarmTypeId
            };
            var id = _farmBusiness.Add(farm);

            return RedirectToAction("Detail", new { id });
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateFarm)]
        public IActionResult Update(int id)
        {
            var farm = _farmBusiness.FindDetail(id);
            var model = new FarmModel()
            {
                Description = farm.Description,
                CreatedDate = farm.CreatedDate,
                UpdatedDate = farm.UpdatedDate,
                UpdatedBy = farm.UpdatedBy,
                UpdateById = farm.UpdatedById,
                CreatedBy = farm.CreatedBy,
                CreatedById = farm.CreatedById,
                FarmTypeId = farm.FarmTypeId,
                FarmTypeName = farm.FarmTypeName,
                Name = farm.Name,
                Id = farm.Id,
                ThumbnailId = farm.ThumbnailId
            };

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateFarm)]
        public async Task<IActionResult> Update(FarmModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var farm = new FarmProjection()
            {
                Id = model.Id,
                UpdatedById = LoggedUserId,
                CreatedById = LoggedUserId,
                Name = model.Name,
                Description = model.Description,
                Thumbnail = new PictureLoadProjection()
                {
                    FileName = model.ThumbnailFileName,
                    ContentType = model.ThumbnailFileType,
                    Base64Data = model.Thumbnail
                },
                FarmTypeId = model.FarmTypeId
            };

            var exist = _farmBusiness.Find(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            farm.UpdatedById = LoggedUserId;
            await _farmBusiness.UpdateAsync(farm);
            return RedirectToAction("Detail", new { id = farm.Id });
        }
    }
}