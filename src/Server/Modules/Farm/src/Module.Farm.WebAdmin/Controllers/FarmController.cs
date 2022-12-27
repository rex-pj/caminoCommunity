using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Farm.WebAdmin.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Camino.Infrastructure.Http.Interfaces;
using Camino.Shared.Configuration.Options;
using Camino.Infrastructure.AspNetCore.Controllers;
using Camino.Application.Contracts.AppServices.Farms;
using Camino.Infrastructure.Identity.Attributes;
using Camino.Shared.Constants;
using Camino.Shared.Enums;
using Camino.Application.Contracts.AppServices.Farms.Dtos;
using Camino.Infrastructure.AspNetCore.Models;
using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Media.Dtos;
using Camino.Core.Domains.Media;
using Camino.Shared.File;

namespace Module.Farm.WebAdmin.Controllers
{
    public class FarmController : BaseAuthController
    {
        private readonly IFarmAppService _farmAppService;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;

        public FarmController(IFarmAppService farmAppService, IHttpHelper httpHelper,
            IHttpContextAccessor httpContextAccessor, IOptions<PagerOptions> pagerOptions)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _farmAppService = farmAppService;
            _pagerOptions = pagerOptions.Value;
        }

        [ApplicationAuthorize(AuthorizePolicies.CanReadFarm)]
        [PopulatePermissions("Farm", PolicyMethods.CanCreate, PolicyMethods.CanUpdate, PolicyMethods.CanDelete)]
        public async Task<IActionResult> Index(FarmFilterModel filter)
        {
            var farmPageList = await _farmAppService.GetAsync(new FarmFilter
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search,
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
                StatusId = (FarmStatuses)x.StatusId
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

        [ApplicationAuthorize(AuthorizePolicies.CanReadFarm)]
        [PopulatePermissions("Farm", PolicyMethods.CanUpdate)]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var farm = await _farmAppService.FindDetailAsync(new IdRequestFilter<long>
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
                    StatusId = (FarmStatuses)farm.StatusId,
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
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateFarm)]
        public async Task<IActionResult> Update(int id)
        {
            var farm = await _farmAppService.FindDetailAsync(new IdRequestFilter<long>
            {
                Id = id,
                CanGetDeleted = true,
                CanGetInactived = true
            });
            if (farm == null)
            {
                return RedirectToNotFoundPage();
            }
            var model = new UpdateFarmModel()
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
                StatusId = (FarmStatuses)farm.StatusId,
                Pictures = farm.Pictures.Select(y => new PictureRequestModel()
                {
                    PictureId = y.Id
                }),
            };

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateFarm)]
        public async Task<IActionResult> Update(UpdateFarmModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = await _farmAppService.FindAsync(new IdRequestFilter<long>
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
                var pictures = new List<PictureRequest>();
                foreach (var picture in model.Pictures)
                {
                    var fileData = await FileUtils.GetBytesAsync(picture.File);
                    pictures.Add(new PictureRequest
                    {
                        BinaryData = fileData,
                        ContentType = picture.File.ContentType,
                        FileName = picture.File.FileName,
                        Id = picture.PictureId.GetValueOrDefault()
                    });
                }

                farm.Pictures = pictures;
            }

            farm.UpdatedById = LoggedUserId;
            await _farmAppService.UpdateAsync(farm);
            return RedirectToAction(nameof(Detail), new { id = farm.Id });
        }

        [HttpDelete]
        [ApplicationAuthorize(AuthorizePolicies.CanDeleteFarm)]
        public async Task<IActionResult> Delete(FarmIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isDeleted = await _farmAppService.DeleteAsync(request.Id);
            if (!isDeleted)
            {
                return RedirectToErrorPage();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateFarm)]
        public async Task<IActionResult> TemporaryDelete(FarmIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isDeleted = await _farmAppService.SoftDeleteAsync(new FarmModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

            if (!isDeleted)
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
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateFarm)]
        public async Task<IActionResult> Deactivate(FarmIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isInactived = await _farmAppService.DeactivateAsync(new FarmModifyRequest
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
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateFarm)]
        public async Task<IActionResult> Active(FarmIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _farmAppService.ActivateAsync(new FarmModifyRequest
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

        [ApplicationAuthorize(AuthorizePolicies.CanReadPicture)]
        [PopulatePermissions("Picture", PolicyMethods.CanCreate, PolicyMethods.CanUpdate, PolicyMethods.CanDelete)]
        public async Task<IActionResult> Pictures(FarmPictureFilterModel filter)
        {
            var filterRequest = new FarmPictureFilter()
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search,
                MimeType = filter.MimeType
            };

            var farmPicturePageList = await _farmAppService.GetPicturesAsync(filterRequest);
            var farmPictures = farmPicturePageList.Collections.Select(x => new FarmPictureModel
            {
                FarmName = x.FarmName,
                FarmId = x.FarmId,
                PictureId = x.PictureId,
                PictureName = x.PictureName,
                PictureCreatedBy = x.PictureCreatedBy,
                PictureCreatedById = x.PictureCreatedById,
                PictureCreatedDate = x.PictureCreatedDate,
                FarmPictureType = (FarmPictureTypes)x.FarmPictureTypeId,
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
        [ApplicationAuthorize(AuthorizePolicies.CanReadFarm)]
        public IActionResult SearchStatus(string q, int? currentId = null)
        {
            var statuses = _farmAppService.SearchStatus(new IdRequestFilter<int?>
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