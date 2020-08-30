using AutoMapper;
using Camino.Service.Data.Content;
using Camino.Service.Data.Filters;
using Camino.Data.Contracts;
using LinqToDB;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Articles.Contracts;
using Camino.DAL.Entities;
using Camino.IdentityDAL.Entities;
using Camino.Service.Data.PageList;
using Camino.Data.Enums;

namespace Camino.Service.Business.Articles
{
    public class ArticleBusiness : IArticleBusiness
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<ArticlePicture> _articlePictureRepository;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<ArticleCategory> _articleCategoryRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public ArticleBusiness(IMapper mapper, IRepository<Article> articleRepository,
            IRepository<ArticleCategory> articleCategoryRepository, IRepository<User> userRepository,
            IRepository<Picture> pictureRepository, IRepository<ArticlePicture> articlePictureRepository)
        {
            _mapper = mapper;
            _articleRepository = articleRepository;
            _articleCategoryRepository = articleCategoryRepository;
            _userRepository = userRepository;
            _pictureRepository = pictureRepository;
            _articlePictureRepository = articlePictureRepository;
        }

        public ArticleProjection Find(long id)
        {
            var exist = (from article in _articleRepository.Table
                         join category in _articleCategoryRepository.Table
                         on article.ArticleCategoryId equals category.Id
                         where article.Id == id
                         select new ArticleProjection
                         {
                             CreatedDate = article.CreatedDate,
                             CreatedById = article.CreatedById,
                             Id = article.Id,
                             Name = article.Name,
                             UpdatedById = article.UpdatedById,
                             UpdatedDate = article.UpdatedDate,
                             ArticleCategoryName = category.Name,
                             ArticleCategoryId = article.ArticleCategoryId
                         }).FirstOrDefault();

            if (exist == null)
            {
                return null;
            }

            var articleResult = _mapper.Map<ArticleProjection>(exist);
            return articleResult;
        }

        public ArticleProjection FindDetail(long id)
        {
            var exist = (from article in _articleRepository.Table
                         join category in _articleCategoryRepository.Table
                         on article.ArticleCategoryId equals category.Id
                         where article.Id == id
                         select new ArticleProjection
                         {
                             Description = article.Description,
                             CreatedDate = article.CreatedDate,
                             CreatedById = article.CreatedById,
                             Id = article.Id,
                             Name = article.Name,
                             UpdatedById = article.UpdatedById,
                             UpdatedDate = article.UpdatedDate,
                             ArticleCategoryName = category.Name,
                             ArticleCategoryId = article.ArticleCategoryId,
                             Content = article.Content
                         }).FirstOrDefault();

            if (exist == null)
            {
                return null;
            }

            var pictureTypeId = (int)ArticlePictureType.Thumbnail;
            var articlePictureId = (from articlePic in _articlePictureRepository.Get(x => x.ArticleId == id && x.PictureType == pictureTypeId)
                                   join pic in _pictureRepository.Table
                                   on articlePic.PictureId equals pic.Id
                                   orderby pic.CreatedDate descending
                                   select pic.Id).FirstOrDefault();

            if (articlePictureId > 0)
            {
                exist.ThumbnailId = articlePictureId;
            }

            var createdByUser = _userRepository.FirstOrDefault(x => x.Id == exist.CreatedById);
            var updatedByUser = _userRepository.FirstOrDefault(x => x.Id == exist.UpdatedById);

            var articleResult = _mapper.Map<ArticleProjection>(exist);
            articleResult.CreatedBy = createdByUser.DisplayName;
            articleResult.UpdatedBy = updatedByUser.DisplayName;

            return articleResult;
        }

        public ArticleProjection FindByName(string name)
        {
            var exist = _articleRepository.Get(x => x.Name == name)
                .FirstOrDefault();

            var article = _mapper.Map<ArticleProjection>(exist);

            return article;
        }

        public async Task<BasePageList<ArticleProjection>> GetAsync(ArticleFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var articleQuery = _articleRepository.Table;
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

            var query = articleQuery.Select(a => new ArticleProjection
            {
                Id = a.Id,
                Name = a.Name,
                CreatedById = a.CreatedById,
                CreatedDate = a.CreatedDate,
                Description = a.Description,
                UpdatedById = a.UpdatedById,
                UpdatedDate = a.UpdatedDate
            });

            var filteredNumber = query.Select(x => x.Id).Count();

            var articles = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var createdByIds = articles.Select(x => x.CreatedById).ToArray();
            var updatedByIds = articles.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).ToList();
            var updatedByUsers = _userRepository.Get(x => updatedByIds.Contains(x.Id)).ToList();

            foreach (var category in articles)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.UpdatedBy = updatedBy.DisplayName;
            }


            var result = new BasePageList<ArticleProjection>(articles);
            result.TotalResult = filteredNumber;
            result.TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize);
            return result;
        }

        public int Add(ArticleProjection article)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            var newArticle = _mapper.Map<Article>(article);
            newArticle.UpdatedDate = modifiedDate;
            newArticle.CreatedDate = modifiedDate;

            var id = _articleRepository.AddWithInt32Entity(newArticle);
            if (!string.IsNullOrEmpty(article.Thumbnail))
            {
                var pictureData = Convert.FromBase64String(article.Thumbnail);
                var pictureId = _pictureRepository.AddWithInt64Entity(new Picture()
                {
                    CreatedById = article.UpdatedById,
                    CreatedDate = modifiedDate,
                    FileName = article.ThumbnailFileName,
                    MimeType = article.ThumbnailFileType,
                    UpdatedById = article.UpdatedById,
                    UpdatedDate = modifiedDate,
                    BinaryData = pictureData
                });

                _articlePictureRepository.Add(new ArticlePicture()
                {
                    ArticleId = id,
                    PictureId = pictureId,
                    PictureType = (int)ArticlePictureType.Thumbnail
                });
            }

            return (int)id;
        }

        public ArticleProjection Update(ArticleProjection article)
        {
            var updatedDate = DateTimeOffset.UtcNow;
            var exist = _articleRepository.FirstOrDefault(x => x.Id == article.Id);
            exist.Description = article.Description;
            exist.Name = article.Name;
            exist.ArticleCategoryId = article.ArticleCategoryId;
            exist.UpdatedById = article.UpdatedById;
            exist.UpdatedDate = updatedDate;
            exist.Content = article.Content;
            if (!string.IsNullOrEmpty(article.Thumbnail))
            {
                var pictureData = Convert.FromBase64String(article.Thumbnail);
                var pictureId = _pictureRepository.AddWithInt64Entity(new Picture() { 
                    CreatedById = article.UpdatedById,
                    CreatedDate = updatedDate,
                    FileName = article.ThumbnailFileName,
                    MimeType = article.ThumbnailFileType,
                    UpdatedById = article.UpdatedById,
                    UpdatedDate = updatedDate,
                    BinaryData = pictureData
                });

                _articlePictureRepository.Add(new ArticlePicture()
                {
                    ArticleId = exist.Id,
                    PictureId = pictureId,
                    PictureType = (int)ArticlePictureType.Thumbnail
                });
            }

            _articleRepository.Update(exist);

            article.UpdatedDate = exist.UpdatedDate;
            return article;
        }
    }
}
