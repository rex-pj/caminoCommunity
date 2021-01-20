using Camino.Core.Constants;
using Camino.Core.Enums;
using Camino.Data.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Helpers.Contracts;
using Camino.Framework.Models;
using Camino.Service.Business.Farms.Contracts;
using Camino.Service.Projections.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.FarmManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Web.FarmManagement.Controllers
{
    public class FarmPictureController : BaseAuthController
    {
        private readonly IFarmPictureBusiness _farmPictureBusiness;
        private readonly IHttpHelper _httpHelper;

        public FarmPictureController(IHttpContextAccessor httpContextAccessor, IFarmPictureBusiness farmPictureBusiness,
            IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _farmPictureBusiness = farmPictureBusiness;
            _httpHelper = httpHelper;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadPicture)]
        [LoadResultAuthorizations("Picture", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(FarmPictureFilterModel filter)
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

            var farmPicturePageList = await _farmPictureBusiness.GetAsync(filterRequest);
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
                return PartialView("_FarmPictureTable", farmPage);
            }

            return View(farmPage);
        }
    }
}
