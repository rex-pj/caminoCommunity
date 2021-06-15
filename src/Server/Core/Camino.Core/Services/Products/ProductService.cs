using Camino.Shared.Requests.Filters;
using System.Threading.Tasks;
using Camino.Core.Contracts.Services.Products;
using Camino.Shared.Results.PageList;
using System.Collections.Generic;
using Camino.Shared.Results.Products;
using Camino.Shared.Requests.Products;
using Camino.Core.Contracts.Repositories.Products;
using System.Linq;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Shared.Results.Media;
using Camino.Shared.Enums;
using System;

namespace Camino.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductPictureRepository _productPictureRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserPhotoRepository _userPhotoRepository;
        private readonly IProductAttributeRepository _productAttributeRepository;

        public ProductService(IProductRepository productRepository, IProductPictureRepository productPictureRepository,
            IUserRepository userRepository, IUserPhotoRepository userPhotoRepository, IProductAttributeRepository productAttributeRepository)
        {
            _productRepository = productRepository;
            _productPictureRepository = productPictureRepository;
            _userRepository = userRepository;
            _userPhotoRepository = userPhotoRepository;
            _productAttributeRepository = productAttributeRepository;
        }

        public async Task<ProductResult> FindAsync(long id)
        {
            return await _productRepository.FindAsync(id);
        }

        public async Task<ProductResult> FindDetailAsync(long id)
        {
            var product = await _productRepository.FindDetailAsync(id);
            if (product == null)
            {
                return null;
            }

            var pictures = await _productPictureRepository.GetProductPicturesByProductIdAsync(id);
            product.Pictures = pictures.Select(x => new PictureResult
            {
                Id = x.PictureId
            });

            product.ProductAttributes = await _productAttributeRepository.GetAttributeRelationsByProductIdAsync(id);

            product.CreatedBy = (await _userRepository.FindByIdAsync(product.CreatedById)).DisplayName;
            product.UpdatedBy = (await _userRepository.FindByIdAsync(product.CreatedById)).DisplayName;
            return product;
        }

        public ProductResult FindByName(string name)
        {
            return _productRepository.FindByName(name);
        }

        public async Task<BasePageList<ProductResult>> GetAsync(ProductFilter filter)
        {
            var productPageList = await _productRepository.GetAsync(filter);
            var createdByIds = productPageList.Collections.Select(x => x.CreatedById).ToArray();
            var updatedByIds = productPageList.Collections.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = await _userRepository.GetNameByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetNameByIdsAsync(updatedByIds);

            var productIds = productPageList.Collections.Select(x => x.Id);
            var pictureTypeId = (int)ProductPictureType.Thumbnail;
            var pictures = await _productPictureRepository.GetProductPicturesByProductIdsAsync(productIds, pictureTypeId);

            var userAvatars = await _userPhotoRepository.GetUserPhotosByUserIds(createdByIds, UserPhotoKind.Avatar);
            foreach (var product in productPageList.Collections)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == product.CreatedById);
                product.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == product.UpdatedById);
                product.UpdatedBy = updatedBy.DisplayName;

                var productPictures = pictures.Where(x => x.ProductId == product.Id);
                if (productPictures != null && productPictures.Any())
                {
                    product.Pictures = productPictures.Select(x => new PictureResult
                    {
                        Id = x.PictureId
                    });
                }

                var avatar = userAvatars.FirstOrDefault(x => x.UserId == product.CreatedById);
                if (avatar != null)
                {
                    product.CreatedByPhotoCode = avatar.Code;
                }
            }

            return productPageList;
        }

        public async Task<IList<ProductResult>> GetRelevantsAsync(long id, ProductFilter filter)
        {
            var products = await _productRepository.GetRelevantsAsync(id, filter);
            var createdByIds = products.Select(x => x.CreatedById).ToArray();
            var updatedByIds = products.Select(x => x.UpdatedById).ToArray();
            var createdByUsers = await _userRepository.GetNameByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetNameByIdsAsync(updatedByIds);

            var productIds = products.Select(x => x.Id);
            var pictureTypeId = (int)ProductPictureType.Thumbnail;
            var pictures = await _productPictureRepository.GetProductPicturesByProductIdsAsync(productIds, pictureTypeId);

            var userAvatars = await _userPhotoRepository.GetUserPhotosByUserIds(createdByIds, UserPhotoKind.Avatar);
            foreach (var product in products)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == product.CreatedById);
                product.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == product.UpdatedById);
                product.UpdatedBy = updatedBy.DisplayName;

                var productPictures = pictures.Where(x => x.ProductId == product.Id);
                if (productPictures != null && productPictures.Any())
                {
                    product.Pictures = productPictures.Select(x => new PictureResult
                    {
                        Id = x.PictureId
                    });
                }

                var avatar = userAvatars.FirstOrDefault(x => x.UserId == product.CreatedById);
                if (avatar != null)
                {
                    product.CreatedByPhotoCode = avatar.Code;
                }
            }

            return products;
        }

        public async Task<long> CreateAsync(ProductModifyRequest request)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            request.CreatedDate = modifiedDate;
            request.UpdatedDate = modifiedDate;
            var id = await _productRepository.CreateAsync(request);
            if (id <= 0)
            {
                return -1;
            }

            if (request.Pictures.Any())
            {
                await _productPictureRepository.CreateAsync(new ProductPicturesModifyRequest
                {
                    CreatedById = request.CreatedById,
                    CreatedDate = request.CreatedDate,
                    ProductId = id,
                    Pictures = request.Pictures,
                    UpdatedById = request.UpdatedById,
                    UpdatedDate = request.UpdatedDate
                });
            }

            if (request.ProductAttributes == null || !request.ProductAttributes.Any())
            {
                return id;
            }

            var groupOfAttributes = new List<ProductAttributeRelationRequest>();
            foreach (var attributeRelation in request.ProductAttributes)
            {
                var existAttribute = groupOfAttributes.FirstOrDefault(x => x.ProductAttributeId == attributeRelation.ProductAttributeId);
                if (existAttribute != null)
                {
                    var attributeValues = existAttribute.AttributeRelationValues.ToList();
                    attributeValues.AddRange(attributeRelation.AttributeRelationValues);
                    existAttribute.AttributeRelationValues = attributeValues;
                }
                else
                {
                    groupOfAttributes.Add(attributeRelation);
                }
            }

            foreach (var attributeRelation in groupOfAttributes)
            {
                attributeRelation.ProductId = id;
                await _productAttributeRepository.CreateAttributeRelationAsync(attributeRelation);
            }

            return id;
        }

        public async Task<bool> UpdateAsync(ProductModifyRequest request)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            request.UpdatedDate = modifiedDate;
            var isUpdated = await _productRepository.UpdateAsync(request);
            if (!isUpdated)
            {
                return false;
            }

            await _productPictureRepository.UpdateAsync(new ProductPicturesModifyRequest
            {
                ProductId = request.Id,
                CreatedById = request.CreatedById,
                UpdatedById = request.UpdatedById,
                CreatedDate = modifiedDate,
                UpdatedDate = modifiedDate,
                Pictures = request.Pictures
            });

            // Delete all product attributes in cases no product attributes from the request
            if (request.ProductAttributes == null || !request.ProductAttributes.Any())
            {
                await _productAttributeRepository.DeleteAttributeRelationByProductIdAsync(request.Id);
                return isUpdated;
            }

            var groupOfAttributes = new List<ProductAttributeRelationRequest>();
            foreach (var attributeRelation in request.ProductAttributes)
            {
                var existAttribute = groupOfAttributes.FirstOrDefault(x => x.ProductAttributeId == attributeRelation.ProductAttributeId);
                if (existAttribute != null)
                {
                    var attributeValues = existAttribute.AttributeRelationValues.ToList();
                    attributeValues.AddRange(attributeRelation.AttributeRelationValues);

                    existAttribute.AttributeRelationValues = attributeValues;
                }
                else
                {
                    groupOfAttributes.Add(attributeRelation);
                }
            }

            var productAttributeIds = request.ProductAttributes.Where(x => x.Id != 0).Select(x => x.Id);
            await _productAttributeRepository.DeleteAttributeRelationNotInIdsAsync(request.Id, productAttributeIds);

            foreach (var attributeRelation in request.ProductAttributes)
            {
                attributeRelation.ProductId = request.Id;
                var isAttributeRelationExist = attributeRelation.Id != 0 && await _productAttributeRepository.IsAttributeRelationExistAsync(attributeRelation.Id);
                if (!isAttributeRelationExist)
                {
                    await _productAttributeRepository.CreateAttributeRelationAsync(attributeRelation);
                }
                else
                {
                    await _productAttributeRepository.UpdateAttributeRelationAsync(attributeRelation);
                }
            }

            return isUpdated;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            await _productPictureRepository.DeleteByProductIdAsync(id);
            await _productAttributeRepository.DeleteAttributeRelationByProductIdAsync(id);
            return await _productRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(ProductModifyRequest request)
        {
            await _productPictureRepository.UpdateStatusByProductIdAsync(new ProductPicturesModifyRequest
            {
                UpdatedById = request.UpdatedById,
                ProductId = request.Id
            }, PictureStatus.Deleted);
            return await _productRepository.SoftDeleteAsync(request);
        }

        public async Task<bool> DeactivateAsync(ProductModifyRequest request)
        {
            await _productPictureRepository.UpdateStatusByProductIdAsync(new ProductPicturesModifyRequest
            {
                UpdatedById = request.UpdatedById,
                ProductId = request.Id
            }, PictureStatus.Inactived);
            return await _productRepository.DeactivateAsync(request);
        }

        public async Task<bool> ActiveAsync(ProductModifyRequest request)
        {
            await _productPictureRepository.UpdateStatusByProductIdAsync(new ProductPicturesModifyRequest
            {
                UpdatedById = request.UpdatedById,
                ProductId = request.Id
            }, PictureStatus.Actived);
            return await _productRepository.ActiveAsync(request);
        }

        public async Task<BasePageList<ProductPictureResult>> GetPicturesAsync(ProductPictureFilter filter)
        {
            var productPicturesPageList = await _productPictureRepository.GetAsync(filter);
            var createdByIds = productPicturesPageList.Collections.GroupBy(x => x.PictureCreatedById).Select(x => x.Key);
            var createdByUsers = await _userRepository.GetNameByIdsAsync(createdByIds);

            foreach (var productPicture in productPicturesPageList.Collections)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == productPicture.PictureCreatedById);
                productPicture.PictureCreatedBy = createdBy.DisplayName;
            }

            return productPicturesPageList;
        }
    }
}
