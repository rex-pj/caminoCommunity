using Camino.Core.Constants;
using Camino.Core.Enums;
using Camino.Data.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Helpers.Contracts;
using Camino.Framework.Models;
using Camino.Service.Business.Articles.Contracts;
using Camino.Service.Projections.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.ArticleManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Web.ArticleManagement.Controllers
{
    public class ArticlePictureController : BaseAuthController
    {
        private readonly IArticlePictureBusiness _articlePictureBusiness;
        private readonly IHttpHelper _httpHelper;

        public ArticlePictureController(IHttpContextAccessor httpContextAccessor, IArticlePictureBusiness articlePictureBusiness,
            IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _articlePictureBusiness = articlePictureBusiness;
            _httpHelper = httpHelper;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadPicture)]
        [LoadResultAuthorizations("Picture", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(ArticlePictureFilterModel filter)
        {
            var filterRequest = new ArticlePictureFilter()
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search,
                MimeType = filter.MimeType
            };

            var articlePicturePageList = await _articlePictureBusiness.GetAsync(filterRequest);
            var articlePictures = articlePicturePageList.Collections.Select(x => new ArticlePictureModel
            {
                ArticleName = x.ArticleName,
                ArticleId = x.ArticleId,
                PictureId = x.PictureId,
                PictureName = x.PictureName,
                PictureCreatedBy = x.PictureCreatedBy,
                PictureCreatedById = x.PictureCreatedById,
                PictureCreatedDate = x.PictureCreatedDate,
                ArticlePictureType = (ArticlePictureType)x.ArticlePictureType,
                ContentType = x.ContentType
            });

            var articlePage = new PageListModel<ArticlePictureModel>(articlePictures)
            {
                Filter = filter,
                TotalPage = articlePicturePageList.TotalPage,
                TotalResult = articlePicturePageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_ArticlePictureTable", articlePage);
            }

            return View(articlePage);
        }
    }
}
