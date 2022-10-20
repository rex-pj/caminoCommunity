using Camino.Shared.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.ArticleManagement.Models;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Camino.Application.Contracts.AppServices.Articles;
using Camino.Infrastructure.Http.Interfaces;
using Camino.Shared.Configuration.Options;
using Camino.Shared.Constants;
using Camino.Application.Contracts.AppServices.Articles.Dtos;
using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Media.Dtos;

namespace Module.Web.ArticleManagement.Controllers
{
    public class ArticleController : BaseAuthController
    {
        private readonly IArticleAppService _articleAppService;
        private readonly IArticlePictureAppService _articlePictureAppService;
        private readonly IHttpHelper _httpHelper;
        private readonly PagerOptions _pagerOptions;

        public ArticleController(IArticleAppService articleAppService, IHttpHelper httpHelper,
            IArticlePictureAppService articlePictureAppService,
            IHttpContextAccessor httpContextAccessor, IOptions<PagerOptions> pagerOptions)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _articleAppService = articleAppService;
            _pagerOptions = pagerOptions.Value;
            _articlePictureAppService = articlePictureAppService;
        }

        [ApplicationAuthorize(AuthorizePolicies.CanReadArticle)]
        [PopulatePermissions("Article", PolicyMethods.CanCreate, PolicyMethods.CanUpdate, PolicyMethods.CanDelete)]
        public async Task<IActionResult> Index(ArticleFilterModel filter)
        {
            var articlePageList = await _articleAppService.GetAsync(new ArticleFilter
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search,
                UpdatedById = filter.UpdatedById,
                CategoryId = filter.CategoryId,
                StatusId = filter.StatusId,
                CanGetDeleted = true,
                CanGetInactived = true
            });

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
                StatusId = (ArticleStatuses)x.StatusId,
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
                return PartialView("Partial/_ArticleTable", articlePage);
            }

            return View(articlePage);
        }

        [ApplicationAuthorize(AuthorizePolicies.CanReadArticle)]
        [PopulatePermissions("Article", PolicyMethods.CanUpdate)]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var article = await _articleAppService.FindDetailAsync(new IdRequestFilter<long>
                {
                    Id = id,
                    CanGetDeleted = true,
                    CanGetInactived = true
                });
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
                    CreatedBy = article.CreatedBy,
                    StatusId = (ArticleStatuses)article.StatusId
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicies.CanCreateArticle)]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new ArticleModel();
            return View(model);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateArticle)]
        public async Task<IActionResult> Update(int id)
        {
            var article = await _articleAppService.FindDetailAsync(new IdRequestFilter<long>
            {
                Id = id,
                CanGetDeleted = true,
                CanGetInactived = true
            });
            if (article == null)
            {
                return RedirectToNotFoundPage();
            }
            var model = new UpdateArticleModel
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
                StatusId = (ArticleStatuses)article.StatusId
            };

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateArticle)]
        public async Task<IActionResult> Update(UpdateArticleModel model)
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

            var exist = await _articleAppService.FindAsync(new IdRequestFilter<long>
            {
                Id = article.Id,
                CanGetDeleted = true,
                CanGetInactived = true
            });
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            await _articleAppService.UpdateAsync(article);
            return RedirectToAction(nameof(Detail), new { id = article.Id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanDeleteArticle)]
        public async Task<IActionResult> Delete(ArticleIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isDeleted = await _articleAppService.DeleteAsync(request.Id);
            if (!isDeleted)
            {
                return RedirectToErrorPage();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateArticle)]
        public async Task<IActionResult> TemporaryDelete(ArticleIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isDeleted = await _articleAppService.SoftDeleteAsync(new ArticleModifyRequest
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
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateArticle)]
        public async Task<IActionResult> Deactivate(ArticleIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isInactived = await _articleAppService.DeactivateAsync(new ArticleModifyRequest
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
        [ApplicationAuthorize(AuthorizePolicies.CanUpdateArticle)]
        public async Task<IActionResult> Active(ArticleIdRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var isActived = await _articleAppService.ActiveAsync(new ArticleModifyRequest
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
        public async Task<IActionResult> Pictures(ArticlePictureFilterModel filter)
        {
            var filterRequest = new ArticlePictureFilter()
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = _pagerOptions.PageSize,
                Keyword = filter.Search,
                MimeType = filter.MimeType
            };

            var articlePicturePageList = await _articlePictureAppService.GetAsync(filterRequest);
            var articlePictures = articlePicturePageList.Collections.Select(x => new ArticlePictureModel
            {
                ArticleName = x.ArticleName,
                ArticleId = x.ArticleId,
                PictureId = x.PictureId,
                PictureName = x.PictureName,
                PictureCreatedBy = x.PictureCreatedBy,
                PictureCreatedById = x.PictureCreatedById,
                PictureCreatedDate = x.PictureCreatedDate,
                ArticlePictureType = (ArticlePictureTypes)x.ArticlePictureTypeId,
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
                return PartialView("Partial/_ArticlePictureTable", articlePage);
            }

            return View(articlePage);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicies.CanReadArticle)]
        public IActionResult SearchStatus(string q, int? currentId = null)
        {
            var statuses = _articleAppService.SearchStatus(new IdRequestFilter<int?>
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