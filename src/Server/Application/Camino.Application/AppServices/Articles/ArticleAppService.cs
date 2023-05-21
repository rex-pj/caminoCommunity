using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Articles;
using Camino.Application.Contracts.AppServices.Articles.Dtos;
using Camino.Application.Contracts.AppServices.Articles.Dtos.Dtos;
using Camino.Application.Contracts.AppServices.Media.Dtos;
using Camino.Application.Contracts.AppServices.Users;
using Camino.Application.Contracts.Utils;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Domains;
using Camino.Core.Domains.Articles;
using Camino.Core.Domains.Articles.DomainServices;
using Camino.Core.Domains.Articles.Repositories;
using Camino.Core.DependencyInjection;
using Camino.Shared.Enums;
using Camino.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Camino.Application.AppServices.Articles
{
    public class ArticleAppService : IArticleAppService, IScopedDependency
    {
        private readonly IEntityRepository<Article> _articleEntityRepository;
        private readonly IEntityRepository<ArticleCategory> _articleCategoryEntityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IUserPhotoRepository _userPhotoRepository;
        private readonly IUserAppService _userAppService;
        private readonly IArticlePictureAppService _articlePictureAppService;
        private readonly IArticlePictureDomainService _articlePictureDomainService;
        private readonly int _deletedStatus = ArticleStatuses.Deleted.GetCode();
        private readonly int _inactivedStatus = ArticleStatuses.Inactived.GetCode();

        public ArticleAppService(IEntityRepository<Article> articleEntityRepository,
            IEntityRepository<ArticleCategory> articleCategoryEntityRepository,
            IArticleRepository articleRepository,
            IUserPhotoRepository userPhotoRepository,
            IUserRepository userRepository,
            IUserAppService userAppService,
            IArticlePictureAppService articlePictureAppService,
            IArticlePictureDomainService articlePictureDomainService)
        {
            _articleEntityRepository = articleEntityRepository;
            _articleCategoryEntityRepository = articleCategoryEntityRepository;
            _articleRepository = articleRepository;
            _userRepository = userRepository;
            _userPhotoRepository = userPhotoRepository;
            _userAppService = userAppService;
            _articlePictureAppService = articlePictureAppService;
            _articlePictureDomainService = articlePictureDomainService;
        }

        #region get
        public async Task<ArticleResult> FindAsync(IdRequestFilter<long> filter)
        {
            var existing = await _articleRepository.FindAsync(filter.Id);
            if (existing == null)
            {
                return null;
            }

            if ((existing.StatusId == _deletedStatus && !filter.CanGetDeleted) || (existing.StatusId == _inactivedStatus && !filter.CanGetInactived))
            {
                return null;
            }

            var result = MapEntityToDto(existing);
            var category = await _articleCategoryEntityRepository.FindAsync(x => x.Id == existing.ArticleCategoryId);
            if (category != null)
            {
                result.ArticleCategoryName = category.Name;
            }

            return result;
        }

        public async Task<ArticleResult> FindDetailAsync(IdRequestFilter<long> filter)
        {
            var existing = await FindAsync(filter);
            if (existing == null)
            {
                return null;
            }

            var picture = await _articlePictureAppService.GetByArticleIdAsync(new IdRequestFilter<long>
            {
                Id = filter.Id,
                CanGetDeleted = filter.CanGetDeleted,
                CanGetInactived = filter.CanGetInactived
            });

            if (picture != null)
            {
                existing.Picture = new PictureResult
                {
                    Id = picture.PictureId
                };
            }

            var createdByUserName = (await _userAppService.FindByIdAsync(existing.CreatedById)).DisplayName;
            existing.CreatedBy = createdByUserName;

            var updatedByUserName = (await _userAppService.FindByIdAsync(existing.UpdatedById)).DisplayName;
            existing.UpdatedBy = updatedByUserName;
            return existing;
        }

        public async Task<ArticleResult> FindByNameAsync(string name)
        {
            var existing = await _articleRepository.FindByNameAsync(name);
            if (existing == null)
            {
                return null;
            }

            if (existing.StatusId == _deletedStatus || existing.StatusId == _inactivedStatus)
            {
                return null;
            }

            return MapEntityToDto(existing);
        }

        private ArticleResult MapEntityToDto(Article entity)
        {
            return new ArticleResult
            {
                Description = entity.Description,
                CreatedDate = entity.CreatedDate,
                CreatedById = entity.CreatedById,
                Id = entity.Id,
                Name = entity.Name,
                UpdatedById = entity.UpdatedById,
                UpdatedDate = entity.UpdatedDate,
                ArticleCategoryId = entity.ArticleCategoryId,
                Content = entity.Content,
                StatusId = entity.StatusId
            };
        }

        public async Task<BasePageList<ArticleResult>> GetAsync(ArticleFilter filter)
        {
            var deletedStatus = ArticleStatuses.Deleted.GetCode();
            var inactivedStatus = ArticleStatuses.Inactived.GetCode();
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var articleQuery = _articleEntityRepository.Get(x => (x.StatusId == deletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == inactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != deletedStatus && x.StatusId != inactivedStatus));
            if (!string.IsNullOrEmpty(search))
            {
                articleQuery = articleQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            var content = filter.Content != null ? filter.Content.ToLower() : "";
            if (!string.IsNullOrEmpty(content))
            {
                articleQuery = articleQuery.Where(user => user.Content.ToLower().Contains(content));
            }

            if (filter.StatusId.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.StatusId == filter.StatusId);
            }

            if (filter.CreatedById.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.UpdatedById.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.UpdatedById == filter.UpdatedById);
            }

            if (filter.CategoryId.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.ArticleCategoryId == filter.CategoryId);
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                articleQuery = articleQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTime.UtcNow);
            }

            var filteredNumber = articleQuery.Select(x => x.Id).Count();

            var query = from ar in articleQuery
                        select new ArticleResult
                        {
                            Id = ar.Id,
                            Name = ar.Name,
                            CreatedById = ar.CreatedById,
                            CreatedDate = ar.CreatedDate,
                            Description = ar.Description,
                            UpdatedById = ar.UpdatedById,
                            UpdatedDate = ar.UpdatedDate,
                            Content = ar.Content,
                            StatusId = ar.StatusId
                        };

            var articles = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            await PopulateDetailsAsync(articles, filter);
            var result = new BasePageList<ArticleResult>(articles)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task<IList<ArticleResult>> GetRelevantsAsync(long id, ArticleFilter filter)
        {
            var exist = (from ar in _articleEntityRepository.Get(x => x.Id == id && x.StatusId != ArticleStatuses.Deleted.GetCode())
                         select new ArticleResult
                         {
                             Id = ar.Id,
                             CreatedById = ar.CreatedById,
                             UpdatedById = ar.UpdatedById,
                             ArticleCategoryId = ar.ArticleCategoryId
                         }).FirstOrDefault();

            var relevantArticleQuery = (from ar in _articleEntityRepository.Get(x => x.Id != exist.Id && x.StatusId != ArticleStatuses.Deleted.GetCode())
                                        where ar.CreatedById == exist.CreatedById
                                        || ar.ArticleCategoryId == exist.ArticleCategoryId
                                        || ar.UpdatedById == exist.UpdatedById
                                        select new ArticleResult
                                        {
                                            Id = ar.Id,
                                            Name = ar.Name,
                                            CreatedById = ar.CreatedById,
                                            CreatedDate = ar.CreatedDate,
                                            Description = ar.Description,
                                            UpdatedById = ar.UpdatedById,
                                            UpdatedDate = ar.UpdatedDate,
                                            Content = ar.Content,
                                        });

            var relevantArticles = await relevantArticleQuery
                .OrderByDescending(x => x.CreatedDate).Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            await PopulateDetailsAsync(relevantArticles, filter);
            return relevantArticles;
        }

        private async Task PopulateDetailsAsync(IList<ArticleResult> articles, ArticleFilter filter)
        {
            var createdByIds = articles.Select(x => x.CreatedById).ToArray();
            var updatedByIds = articles.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = await _userRepository.GetByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetByIdsAsync(updatedByIds);

            var articleIds = articles.Select(x => x.Id);
            var pictures = await _articlePictureAppService.GetListByArticleIdsAsync(articleIds, new IdRequestFilter<long>
            {
                CanGetDeleted = filter.CanGetDeleted,
                CanGetInactived = filter.CanGetInactived
            });
            var userAvatars = await _userPhotoRepository.GetListByUserIdsAsync(createdByIds, UserPictureTypes.Avatar);
            foreach (var article in articles)
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
                    article.CreatedByPhotoId = avatar.Id;
                }
            }
        }
        #endregion

        #region CRUD
        public async Task<long> CreateAsync(ArticleModifyRequest request)
        {
            var id = await _articleRepository.CreateAsync(new Article
            {
                ArticleCategoryId = request.ArticleCategoryId,
                Content = request.Content,
                CreatedById = request.CreatedById,
                UpdatedById = request.UpdatedById,
                Description = request.Description,
                Name = request.Name,
                StatusId = ArticleStatuses.Pending.GetCode()
            });
            if (request.Picture != null && request.Picture.BinaryData != null)
            {
                var modifiedDate = DateTime.UtcNow;
                await _articlePictureAppService.CreateAsync(new ArticlePictureModifyRequest
                {
                    ArticleId = id,
                    CreatedById = request.CreatedById,
                    UpdatedById = request.UpdatedById,
                    CreatedDate = modifiedDate,
                    UpdatedDate = modifiedDate,
                    Picture = request.Picture
                });
            }

            return id;
        }

        public async Task<bool> UpdateAsync(ArticleModifyRequest request)
        {
            var existing = await _articleRepository.FindAsync(request.Id);
            existing.Name = request.Name;
            existing.ArticleCategoryId = request.ArticleCategoryId;
            existing.UpdatedById = request.UpdatedById;
            existing.Content = request.Content;

            var isUpdated = await _articleRepository.UpdateAsync(existing);
            if (isUpdated && request.Picture != null)
            {
                var modifiedDate = DateTime.UtcNow;
                await _articlePictureAppService.UpdateAsync(new ArticlePictureModifyRequest
                {
                    ArticleId = request.Id,
                    CreatedById = request.CreatedById,
                    UpdatedById = request.UpdatedById,
                    CreatedDate = modifiedDate,
                    UpdatedDate = modifiedDate,
                    Picture = request.Picture
                });
            }

            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await _articlePictureDomainService.DeleteByArticleIdAsync(id);
            return await _articleRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(ArticleModifyRequest request)
        {
            await _articlePictureDomainService.UpdateStatusByArticleIdAsync(request.Id, request.UpdatedById, PictureStatuses.Deleted);
            var existing = await _articleRepository.FindAsync(request.Id);
            existing.StatusId = _deletedStatus;
            existing.UpdatedById = request.UpdatedById;

            return await _articleRepository.UpdateAsync(existing);
        }


        public async Task<bool> DeactivateAsync(ArticleModifyRequest request)
        {
            await _articlePictureDomainService.UpdateStatusByArticleIdAsync(request.Id, request.UpdatedById, PictureStatuses.Inactived);
            var existing = await _articleRepository.FindAsync(request.Id);
            existing.StatusId = _inactivedStatus;
            existing.UpdatedById = request.UpdatedById;

            return await _articleRepository.UpdateAsync(existing);
        }

        public async Task<bool> ActiveAsync(ArticleModifyRequest request)
        {
            await _articlePictureDomainService.UpdateStatusByArticleIdAsync(request.Id, request.UpdatedById, PictureStatuses.Actived);
            var existing = await _articleRepository.FindAsync(request.Id);
            existing.StatusId = ArticleStatuses.Actived.GetCode();
            existing.UpdatedById = request.UpdatedById;

            return await _articleRepository.UpdateAsync(existing);
        }
        #endregion

        #region article status
        public IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "")
        {
            search = search != null ? search.ToLower() : "";
            var result = new List<SelectOption>();
            if (filter.Id.HasValue)
            {
                var selected = (ArticleStatuses)filter.Id;
                result = SelectOptionUtils.ToSelectOptions(selected).ToList();
            }
            else
            {
                result = SelectOptionUtils.ToSelectOptions<ArticleStatuses>().ToList();
            }

            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
            return result;
        }
        #endregion
    }
}
