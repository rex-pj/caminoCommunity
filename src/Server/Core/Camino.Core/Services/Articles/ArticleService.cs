using Camino.Shared.Requests.Filters;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Articles;
using System.Collections.Generic;
using Camino.Shared.Requests.Articles;
using Camino.Core.Contracts.Services.Articles;
using Camino.Core.Contracts.Repositories.Articles;
using System.Linq;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Utils;
using Camino.Shared.Results.Media;
using System;
using Camino.Shared.Enums;
using Camino.Shared.General;
using Camino.Core.Contracts.DependencyInjection;

namespace Camino.Services.Articles
{
    public class ArticleService : IArticleService, IScopedDependency
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IArticlePictureRepository _articlePictureRepository;
        private readonly IUserPhotoRepository _userPhotoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IArticleStatusRepository _articleStatusRepository;

        public ArticleService(IArticleRepository articleRepository, IArticlePictureRepository articlePictureRepository,
            IUserPhotoRepository userPhotoRepository, IUserRepository userRepository, IArticleStatusRepository articleStatusRepository)
        {
            _articleRepository = articleRepository;
            _articlePictureRepository = articlePictureRepository;
            _userRepository = userRepository;
            _userPhotoRepository = userPhotoRepository;
            _articleStatusRepository = articleStatusRepository;
        }

        #region get
        public async Task<ArticleResult> FindAsync(IdRequestFilter<long> filter)
        {
            return await _articleRepository.FindAsync(filter);
        }

        public async Task<ArticleResult> FindDetailAsync(IdRequestFilter<long> filter)
        {
            var article = await _articleRepository.FindDetailAsync(filter);
            if (article == null)
            {
                return null;
            }

            var picture = await _articlePictureRepository.GetArticlePictureByArticleIdAsync(new IdRequestFilter<long>
            {
                Id = filter.Id,
                CanGetDeleted = filter.CanGetDeleted,
                CanGetInactived = filter.CanGetInactived
            });

            if (picture != null)
            {
                article.Picture = new PictureResult
                {
                    Id = picture.PictureId
                };
            }

            var createdByUserName = (await _userRepository.FindByIdAsync(article.CreatedById)).DisplayName;
            article.CreatedBy = createdByUserName;

            var updatedByUserName = (await _userRepository.FindByIdAsync(article.UpdatedById)).DisplayName;
            article.UpdatedBy = updatedByUserName;
            return article;
        }

        public ArticleResult FindByName(string name)
        {
            return _articleRepository.FindByName(name);
        }

        public async Task<BasePageList<ArticleResult>> GetAsync(ArticleFilter filter)
        {
            var articlePageList = await _articleRepository.GetAsync(filter);
            var createdByIds = articlePageList.Collections.Select(x => x.CreatedById).ToArray();
            var updatedByIds = articlePageList.Collections.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = await _userRepository.GetNameByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetNameByIdsAsync(updatedByIds);

            var articleIds = articlePageList.Collections.Select(x => x.Id);
            var pictures = await _articlePictureRepository.GetArticlePicturesByArticleIdsAsync(articleIds, new IdRequestFilter<long>
            {
                CanGetDeleted = filter.CanGetDeleted,
                CanGetInactived = filter.CanGetInactived
            });
            var userAvatars = await _userPhotoRepository.GetUserPhotosByUserIdsAsync(createdByIds, UserPictureType.Avatar);
            foreach (var article in articlePageList.Collections)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == article.CreatedById);
                article.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == article.UpdatedById);
                article.UpdatedBy = updatedBy.DisplayName;
                article.Description = string.IsNullOrEmpty(article.Description) ? HtmlUtil.TrimHtml(article.Content) : article.Description;

                var picture = pictures.FirstOrDefault(x => x.ArticleId == article.Id);
                if (picture != null)
                {
                    article.Picture = new PictureResult { Id = picture.PictureId };
                }

                var avatar = userAvatars.FirstOrDefault(x => x.UserId == article.CreatedById);
                if (avatar != null)
                {
                    article.CreatedByPhotoCode = avatar.Code;
                }
            }

            return articlePageList;
        }

        public async Task<IList<ArticleResult>> GetRelevantsAsync(long id, ArticleFilter filter)
        {
            var articles = await _articleRepository.GetRelevantsAsync(id, filter);
            var createdByIds = articles.Select(x => x.CreatedById).ToArray();
            var createdByUsers = await _userRepository.GetNameByIdsAsync(createdByIds);

            var articleIds = articles.Select(x => x.Id);
            var pictures = await _articlePictureRepository.GetArticlePicturesByArticleIdsAsync(articleIds, new IdRequestFilter<long>
            {
                CanGetDeleted = filter.CanGetDeleted,
                CanGetInactived = filter.CanGetInactived
            });

            foreach (var article in articles)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == article.CreatedById);
                article.CreatedBy = createdBy.DisplayName;

                var picture = pictures.FirstOrDefault(x => x.ArticleId == article.Id);
                if (picture != null)
                {
                    article.Picture = new PictureResult { Id = picture.PictureId };
                }
            }

            return articles;
        }

        #endregion

        #region CRUD
        public async Task<int> CreateAsync(ArticleModifyRequest article)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            article.CreatedDate = modifiedDate;
            article.UpdatedDate = modifiedDate;
            var id = await _articleRepository.CreateAsync(article);
            if (article.Picture != null)
            {
                await _articlePictureRepository.CreateAsync(new ArticlePictureModifyRequest
                {
                    ArticleId = id,
                    CreatedById = article.CreatedById,
                    UpdatedById = article.UpdatedById,
                    CreatedDate = modifiedDate,
                    UpdatedDate = modifiedDate,
                    Picture = article.Picture
                });
            }

            return id;
        }

        public async Task<bool> UpdateAsync(ArticleModifyRequest article)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            article.UpdatedDate = modifiedDate;
            var isUpdated = await _articleRepository.UpdateAsync(article);
            if (isUpdated)
            {
                await _articlePictureRepository.UpdateAsync(new ArticlePictureModifyRequest
                {
                    ArticleId = article.Id,
                    CreatedById = article.CreatedById,
                    UpdatedById = article.UpdatedById,
                    CreatedDate = modifiedDate,
                    UpdatedDate = modifiedDate,
                    Picture = article.Picture
                });
            }

            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await _articlePictureRepository.DeleteByArticleIdAsync(id);
            return await _articleRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(ArticleModifyRequest request)
        {
            await _articlePictureRepository.UpdateStatusByArticleIdAsync(new ArticlePictureModifyRequest
            {
                ArticleId = request.Id,
                UpdatedById = request.UpdatedById
            }, PictureStatus.Deleted);
            return await _articleRepository.SoftDeleteAsync(request);
        }


        public async Task<bool> DeactivateAsync(ArticleModifyRequest request)
        {
            await _articlePictureRepository.UpdateStatusByArticleIdAsync(new ArticlePictureModifyRequest
            {
                ArticleId = request.Id,
                UpdatedById = request.UpdatedById
            }, PictureStatus.Inactived);
            return await _articleRepository.DeactivateAsync(request);
        }

        public async Task<bool> ActiveAsync(ArticleModifyRequest request)
        {
            await _articlePictureRepository.UpdateStatusByArticleIdAsync(new ArticlePictureModifyRequest
            {
                ArticleId = request.Id,
                UpdatedById = request.UpdatedById
            }, PictureStatus.Actived);
            return await _articleRepository.ActiveAsync(request);
        }
        #endregion

        #region article picture
        public async Task<BasePageList<ArticlePictureResult>> GetPicturesAsync(ArticlePictureFilter filter)
        {
            var articlePictureListPage = await _articlePictureRepository.GetAsync(filter);
            var createdByIds = articlePictureListPage.Collections.GroupBy(x => x.PictureCreatedById).Select(x => x.Key);
            var createdByUsers = await _userRepository.GetNameByIdsAsync(createdByIds);

            foreach (var articlePicture in articlePictureListPage.Collections)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == articlePicture.PictureCreatedById);
                articlePicture.PictureCreatedBy = createdBy.DisplayName;
            }

            return articlePictureListPage;
        }
        #endregion

        #region article status
        public IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "")
        {
            return _articleStatusRepository.Search(filter, search);
        }
        #endregion
    }
}
