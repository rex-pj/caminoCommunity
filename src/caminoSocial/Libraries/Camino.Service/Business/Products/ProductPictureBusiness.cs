using Camino.DAL.Entities;
using Camino.Data.Contracts;
using Camino.Service.Business.Products.Contracts;
using System.Threading.Tasks;
using System.Linq;
using LinqToDB;
using Camino.Service.Projections.Filters;
using System;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.PageList;
using Camino.Service.Projections.Product;

namespace Camino.Service.Business.Products
{
    public class ProductPictureBusiness : IProductPictureBusiness
    {
        private readonly IRepository<ProductPicture> _productPictureRepository;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<User> _userRepository;

        public ProductPictureBusiness(IRepository<ProductPicture> productPictureRepository, IRepository<Picture> pictureRepository,
            IRepository<User> userRepository, IRepository<Product> productRepository)
        {
            _productPictureRepository = productPictureRepository;
            _pictureRepository = pictureRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<BasePageList<ProductPictureProjection>> GetAsync(ProductPictureFilter filter)
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

            var query = from ap in _productPictureRepository.Table
                        join p in pictureQuery
                        on ap.PictureId equals p.Id
                        join a in _productRepository.Table
                        on ap.ProductId equals a.Id
                        select new ProductPictureProjection()
                        {
                            ProductId = a.Id,
                            ProductName = a.Name,
                            PictureId = p.Id,
                            PictureName = p.FileName,
                            ProductPictureType = ap.PictureType,
                            PictureCreatedById = p.CreatedById,
                            PictureCreatedDate = p.CreatedDate,
                            ContentType = p.MimeType
                        };

            var filteredNumber = query.Select(x => x.PictureId).Count();

            var productPictures = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var createdByIds = productPictures.GroupBy(x => x.PictureCreatedById).Select(x => x.Key);

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).ToList();

            foreach (var productPicture in productPictures)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == productPicture.PictureCreatedById);
                productPicture.PictureCreatedBy = createdBy.DisplayName;
            }

            var result = new BasePageList<ProductPictureProjection>(productPictures)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }
    }
}
