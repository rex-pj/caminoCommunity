using Camino.DAL.Entities;
using Camino.Data.Contracts;
using Camino.Service.Business.Articles.Contracts;
using System.Threading.Tasks;
using System.Linq;
using LinqToDB;
using Camino.Service.Projections.Filters;
using System;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.PageList;
using Camino.Service.Projections.Article;

namespace Camino.Service.Business.Articles
{
    public class ArticlePictureBusiness : IArticlePictureBusiness
    {
        private readonly IRepository<ArticlePicture> _articlePictureRepository;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<User> _userRepository;

        public ArticlePictureBusiness(IRepository<ArticlePicture> articlePictureRepository, IRepository<Picture> pictureRepository,
            IRepository<User> userRepository, IRepository<Article> articleRepository)
        {
            _articlePictureRepository = articlePictureRepository;
            _pictureRepository = pictureRepository;
            _articleRepository = articleRepository;
            _userRepository = userRepository;
        }

        public async Task<BasePageList<ArticlePictureProjection>> GetAsync(ArticlePictureFilter filter)
        {
            var pictureQuery = _pictureRepository.Table;
            if (!string.IsNullOrEmpty(filter.Search))
            {
                var search = filter.Search.ToLower();
                pictureQuery = pictureQuery.Where(pic => pic.Title.ToLower().Contains(search)
                         || pic.Title.ToLower().Contains(search));
            }

            if (filter.CreatedById.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (!string.IsNullOrEmpty(filter.MimeType))
            {
                var mimeType = filter.MimeType.ToLower();
                pictureQuery = pictureQuery.Where(x => x.MimeType.Contains(mimeType));
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTimeOffset.UtcNow);
            }

            var query = from ap in _articlePictureRepository.Table
                        join p in pictureQuery
                        on ap.PictureId equals p.Id
                        join a in _articleRepository.Table
                        on ap.ArticleId equals a.Id
                        select new ArticlePictureProjection()
                        {
                            ArticleId = a.Id,
                            ArticleName = a.Name,
                            PictureId = p.Id,
                            PictureName = p.FileName,
                            ArticlePictureType = ap.PictureType,
                            PictureCreatedById = p.CreatedById,
                            PictureCreatedDate = p.CreatedDate,
                            ContentType = p.MimeType
                        };

            var filteredNumber = query.Select(x => x.PictureId).Count();

            var articlePictures = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var createdByIds = articlePictures.GroupBy(x => x.PictureCreatedById).Select(x => x.Key);

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).ToList();

            foreach (var articlePicture in articlePictures)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == articlePicture.PictureCreatedById);
                articlePicture.PictureCreatedBy = createdBy.DisplayName;
            }

            var result = new BasePageList<ArticlePictureProjection>(articlePictures)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }
    }
}
