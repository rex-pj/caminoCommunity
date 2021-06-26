using Camino.Core.Constants;
using Camino.Shared.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Core.Contracts.Helpers;
using Camino.Framework.Models;
using Camino.Core.Contracts.Services.Farms;
using Camino.Shared.Requests.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.FarmManagement.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.Requests.Farms;
using Camino.Shared.Requests.Media;
using System.Collections.Generic;

namespace Module.Web.FarmManagement.Controllers
{
    public class FarmController : BaseAuthController
    {
        private readonly IFarmService _farmService;
        private readonly IHttpHelper _httpHelper;

        public FarmController(IFarmService farmService, IHttpHelper httpHelper,
            IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _farmService = farmService;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadFarm)]
        [LoadResultAuthorizations("Farm", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(FarmFilterModel filter)
        {
            var farmPageList = await _farmService.GetAsync(new FarmFilter
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search,
                UpdatedById = filter.UpdatedById,
                FarmTypeId = filter.FarmTypeId,
                StatusId = filter.StatusId,
                CanGetDeleted = true,
                CanGetInactived = true
            });
            var farms = farmPageList.Collections.Select(x => new FarmModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                PictureId = x.Pictures.Any() ? x.Pictures.FirstOrDefault().Id : 0,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                StatusId = (FarmStatus)x.StatusId
            });

            var farmPage = new PageListModel<FarmModel>(farms)
            {
                Filter = filter,
                TotalPage = farmPageList.TotalPage,
                TotalResult = farmPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("Partial/_FarmTable", farmPage);
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
                var farm = await _farmService.FindDetailAsync(new IdRequestFilter<long>
                {
                    Id = id,
                    CanGetDeleted = true,
                    CanGetInactived = true
                });
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
                    StatusId = (FarmStatus)farm.StatusId,
                    Pictures = farm.Pictures.Select(y => new PictureRequestModel()
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
            var farm = await _farmService.FindDetailAsync(new IdRequestFilter<long>
            {
                Id = id,
                CanGetDeleted = true,
                CanGetInactived = true
            });
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
                StatusId = (FarmStatus)farm.StatusId,
                Pictures = farm.Pictures.Select(y => new PictureRequestModel()
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

            var exist = await _farmService.FindAsync(new IdRequestFilter<long>
            {
                Id = model.Id,
                CanGetDeleted = true,
                CanGetInactived = true
            });
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            var farm = new FarmModifyRequest()
            {
                Id = model.Id,
                UpdatedById = LoggedUserId,
                CreatedById = LoggedUserId,
                Name = model.Name,
                Description = model.Description,
                FarmTypeId = model.FarmTypeId
            };

            if (model.Pictures != null && model.Pictures.Any())
            {
                farm.Pictures = model.Pictures.Select(x => new PictureRequest()
                {
                    Base64Data = x.Base64Data,
                    ContentType = x.ContentType,
                    FileName = x.FileName,
                    Id = x.PictureId
                });
            }

            farm.UpdatedById = LoggedUserId;
            await _farmService.UpdateAsync(farm);
            return RedirectToAction(nameof(Detail), new { id = farm.Id });
        }

        [HttpDelete]
        [ApplicationAuthorize(AuthorizePolicyConst.CanDeleteFarm)]
        public async Task<IActionResult> Delete(FarmIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isDeleted = await _farmService.DeleteAsync(request.Id);
            if (!isDeleted)
            {
                return RedirectToErrorPage();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateFarm)]
        public async Task<IActionResult> TemporaryDelete(FarmIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isDeleted = await _farmService.SoftDeleteAsync(new FarmModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

            if (!isDeleted)
            {
                return RedirectToErrorPage();
            }

            if (request.ShouldBackToDetail)
            {
                return RedirectToAction(nameof(Detail), new { id = request.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateFarm)]
        public async Task<IActionResult> Deactivate(FarmIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isInactived = await _farmService.DeactivateAsync(new FarmModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

            if (!isInactived)
            {
                return RedirectToErrorPage();
            }

            if (request.ShouldBackToDetail)
            {
                return RedirectToAction(nameof(Detail), new { id = request.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateFarm)]
        public async Task<IActionResult> Active(FarmIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _farmService.ActivateAsync(new FarmModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

            if (!isActived)
            {
                return RedirectToErrorPage();
            }

            if (request.ShouldBackToDetail)
            {
                return RedirectToAction(nameof(Detail), new { id = request.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadPicture)]
        [LoadResultAuthorizations("Picture", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Pictures(FarmPictureFilterModel filter)
        {
            var filterRequest = new FarmPictureFilter()
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search,
                MimeType = filter.MimeType
            };

            var farmPicturePageList = await _farmService.GetPicturesAsync(filterRequest);
            var farmPictures = farmPicturePageList.Collections.Select(x => new FarmPictureModel
            {
                FarmName = x.FarmName,
                FarmId = x.FarmId,
                PictureId = x.PictureId,
                PictureName = x.PictureName,
                PictureCreatedBy = x.PictureCreatedBy,
                PictureCreatedById = x.PictureCreatedById,
                PictureCreatedDate = x.PictureCreatedDate,
                FarmPictureType = (FarmPictureType)x.FarmPictureTypeId,
                ContentType = x.ContentType
            });

            var farmPage = new PageListModel<FarmPictureModel>(farmPictures)
            {
                Filter = filter,
                TotalPage = farmPicturePageList.TotalPage,
                TotalResult = farmPicturePageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("Partial/_FarmPictureTable", farmPage);
            }

            return View(farmPage);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadFarm)]
        public IActionResult SearchStatus(string q, int? currentId = null)
        {
            var statuses = _farmService.SearchStatus(new IdRequestFilter<int?>
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