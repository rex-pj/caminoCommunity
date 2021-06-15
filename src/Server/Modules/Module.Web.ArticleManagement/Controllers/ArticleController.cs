using Camino.Shared.Requests.Filters;
using Camino.Core.Constants;
using Camino.Shared.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Core.Contracts.Helpers;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.ArticleManagement.Models;
using System;
using System.Threading.Tasks;
using Camino.Shared.Requests.Articles;
using Camino.Core.Contracts.Services.Articles;
using System.Linq;
using Camino.Shared.Requests.Media;

namespace Module.Web.ArticleManagement.Controllers
{
    public class ArticleController : BaseAuthController
    {
        private readonly IArticleService _articleService;
        private readonly IHttpHelper _httpHelper;

        public ArticleController(IArticleService articleService, IHttpHelper httpHelper,
            IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _articleService = articleService;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadArticle)]
        [LoadResultAuthorizations("Article", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(ArticleFilterModel filter)
        {
            var filterRequest = new ArticleFilter
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search,
                UpdatedById = filter.UpdatedById,
                CategoryId = filter.CategoryId,
                IsGettingDeleted = true
            };

            var articlePageList = await _articleService.GetAsync(filterRequest);
            var articles = articlePageList.Collections.Select(x => new ArticleModel
            {
                Id = x.Id,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate,
                CreatedById = x.CreatedById,
                ArticleCategoryId = x.ArticleCategoryId,
                ArticleCategoryName = x.ArticleCategoryName,
                Content = x.Content,
                Description = x.Description,
                Name = x.Name,
                PictureId = x.Picture.Id,
                StatusId = (ArticleStatus)x.StatusId,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy
            });
            var articlePage = new PageListModel<ArticleModel>(articles)
            {
                Filter = filter,
                TotalPage = articlePageList.TotalPage,
                TotalResult = articlePageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_ArticleTable", articlePage);
            }

            return View(articlePage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadArticle)]
        [LoadResultAuthorizations("Article", PolicyMethod.CanUpdate)]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var article = await _articleService.FindDetailAsync(id);
                if (article == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new ArticleModel
                {
                    Id = article.Id,
                    CreatedDate = article.CreatedDate,
                    CreatedById = article.CreatedById,
                    ArticleCategoryId = article.ArticleCategoryId,
                    ArticleCategoryName = article.ArticleCategoryName,
                    Content = article.Content,
                    Description = article.Description,
                    Name = article.Name,
                    PictureId = article.Picture.Id,
                    UpdateById = article.UpdatedById,
                    UpdatedDate = article.UpdatedDate,
                    UpdatedBy = article.UpdatedBy,
                    CreatedBy = article.CreatedBy
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateArticle)]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new ArticleModel();
            return View(model);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticle)]
        public async Task<IActionResult> Update(int id)
        {
            var article = await _articleService.FindDetailAsync(id);
            var model = new ArticleModel
            {
                Id = article.Id,
                CreatedDate = article.CreatedDate,
                CreatedById = article.CreatedById,
                ArticleCategoryId = article.ArticleCategoryId,
                ArticleCategoryName = article.ArticleCategoryName,
                Content = article.Content,
                Description = article.Description,
                Name = article.Name,
                PictureId = article.Picture.Id,
                UpdateById = article.UpdatedById,
                UpdatedDate = article.UpdatedDate
            };

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticle)]
        public async Task<IActionResult> Update(ArticleModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var article = new ArticleModifyRequest
            {
                Id = model.Id,
                CreatedById = model.CreatedById,
                ArticleCategoryId = model.ArticleCategoryId,
                Content = model.Content,
                Description = model.Description,
                Name = model.Name,
                Picture = new PictureRequest
                {
                    Base64Data = model.Picture,
                    ContentType = model.PictureFileType,
                    FileName = model.PictureFileName
                },
                UpdatedById = LoggedUserId,
            };
            if (article.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = await _articleService.FindAsync(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            await _articleService.UpdateAsync(article);
            return RedirectToAction(nameof(Detail), new { id = article.Id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanDeleteArticle)]
        public async Task<IActionResult> Delete(ArticleIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isDeleted = await _articleService.DeleteAsync(request.Id);
            if (!isDeleted)
            {
                return RedirectToErrorPage();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticle)]
        public async Task<IActionResult> TemporaryDelete(ArticleIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isDeleted = await _articleService.SoftDeleteAsync(new ArticleModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });
            if (!isDeleted)
            {
                return RedirectToErrorPage();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticle)]
        public async Task<IActionResult> Deactivate(ArticleIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isInactived = await _articleService.DeactivateAsync(new ArticleModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });

            if (!isInactived)
            {
                return RedirectToErrorPage();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticle)]
        public async Task<IActionResult> Active(ArticleIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _articleService.ActiveAsync(new ArticleModifyRequest
            {
                Id = request.Id,
                UpdatedById = LoggedUserId
            });
            if (!isActived)
            {
                return RedirectToErrorPage();
            }

            return RedirectToAction(nameof(Index));
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadPicture)]
        [LoadResultAuthorizations("Picture", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Pictures(ArticlePictureFilterModel filter)
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

            var articlePicturePageList = await _articleService.GetPicturesAsync(filterRequest);
            var articlePictures = articlePicturePageList.Collections.Select(x => new ArticlePictureModel
            {
                ArticleName = x.ArticleName,
                ArticleId = x.ArticleId,
                PictureId = x.PictureId,
                PictureName = x.PictureName,
                PictureCreatedBy = x.PictureCreatedBy,
                PictureCreatedById = x.PictureCreatedById,
                PictureCreatedDate = x.PictureCreatedDate,
                ArticlePictureType = (ArticlePictureType)x.ArticlePictureTypeId,
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