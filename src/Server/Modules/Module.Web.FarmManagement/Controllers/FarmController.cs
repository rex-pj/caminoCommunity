using Camino.Core.Constants;
using Camino.Core.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Helpers.Contracts;
using Camino.Framework.Models;
using Camino.Service.Business.Farms.Contracts;
using Camino.Service.Projections.Farm;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Media;
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
                ThumbnailId = x.Pictures.Any() ? x.Pictures.FirstOrDefault().Id : 0,
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
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var farm = await _farmBusiness.FindDetailAsync(id);
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
                    Thumbnails = farm.Pictures.Select(y => new PictureRequestModel()
                    {
                        PictureId = y.Id
                    }),
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateFarm)]
        public async Task<IActionResult> Update(int id)
        {
            var farm = await _farmBusiness.FindDetailAsync(id);
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
                Thumbnails = farm.Pictures.Select(y => new PictureRequestModel()
                {
                    PictureId = y.Id
                }),
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

            var exist = await _farmBusiness.FindAsync(model.Id);
            if (exist == null)
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
                FarmTypeId = model.FarmTypeId
            };

            if (model.Thumbnails != null && model.Thumbnails.Any())
            {
                farm.Pictures = model.Thumbnails.Select(x => new PictureRequestProjection()
                {
                    Base64Data = x.Base64Data,
                    ContentType = x.ContentType,
                    FileName = x.FileName,
                    Id = x.PictureId
                });
            }

            farm.UpdatedById = LoggedUserId;
            await _farmBusiness.UpdateAsync(farm);
            return RedirectToAction("Detail", new { id = farm.Id });
        }
    }
}