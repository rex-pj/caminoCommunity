using Camino.Core.Domains.Farms.Repositories;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Shared.Enums;
using Camino.Application.Contracts;
using Camino.Core.Domains.Farms;
using Camino.Shared.Utils;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains;
using Microsoft.EntityFrameworkCore;
using Camino.Core.Domains.Products;
using Camino.Core.Domains.Products.DomainServices;
using Camino.Application.Contracts.AppServices.Farms;
using Camino.Application.Contracts.AppServices.Farms.Dtos;
using Camino.Application.Contracts.AppServices.Media.Dtos;
using Camino.Application.Contracts.Utils;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Camino.Application.AppServices.Farms
{
    public class FarmAppService : IFarmAppService, IScopedDependency
    {
        private readonly IFarmRepository _farmRepository;
        private readonly IEntityRepository<Farm> _farmEntityRepository;
        private readonly IEntityRepository<FarmProduct> _farmProductEntityRepository;
        private readonly IEntityRepository<Product> _productEntityRepository;
        private readonly IEntityRepository<ProductPrice> _productPriceEntityRepository;
        private readonly IEntityRepository<ProductCategoryRelation> _productCategoryRelationEntityRepository;
        private readonly IProductPictureDomainService _productPictureDomainService;
        private readonly IFarmPictureAppService _farmPictureAppService;
        private readonly IUserRepository _userRepository;
        private readonly IUserPhotoRepository _userPhotoRepository;
        private readonly int _deletedStatus = FarmStatuses.Deleted.GetCode();
        private readonly int _inactivedStatus = FarmStatuses.Inactived.GetCode();
        private readonly IDbContext _dbContext;

        public FarmAppService(IFarmRepository farmRepository,
            IFarmPictureAppService farmPictureAppService, IUserRepository userRepository,
            IEntityRepository<Farm> farmEntityRepository,
            IEntityRepository<FarmProduct> farmProductEntityRepository,
            IEntityRepository<Product> productEntityRepository,
            IEntityRepository<ProductPrice> productPriceEntityRepository,
            IEntityRepository<ProductCategoryRelation> productCategoryRelationEntityRepository,
            IProductPictureDomainService productPictureDomainService,
            IUserPhotoRepository userPhotoRepository,
            IDbContext dbContext)
        {
            _farmRepository = farmRepository;
            _farmPictureAppService = farmPictureAppService;
            _userRepository = userRepository;
            _userPhotoRepository = userPhotoRepository;
            _farmEntityRepository = farmEntityRepository;

            _farmProductEntityRepository = farmProductEntityRepository;
            _productEntityRepository = productEntityRepository;
            _productPriceEntityRepository = productPriceEntityRepository;
            _productCategoryRelationEntityRepository = productCategoryRelationEntityRepository;
            _productPictureDomainService = productPictureDomainService;
            _dbContext = dbContext;
        }

        #region get
        public async Task<FarmResult> FindAsync(IdRequestFilter<long> filter)
        {
            var existing = await _farmRepository.FindAsync(filter.Id);
            if (existing == null)
            {
                return null;
            }

            if ((existing.StatusId == _deletedStatus && !filter.CanGetDeleted) || (existing.StatusId == _inactivedStatus && !filter.CanGetInactived))
            {
                return null;
            }

            return MapEntityToDto(existing);
        }

        public async Task<FarmResult> FindByNameAsync(string name)
        {
            var existing = await _farmRepository.FindByNameAsync(name);
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

        public async Task<IList<FarmResult>> GetByTypeAsync(IdRequestFilter<long> filter)
        {
            var farms = await _farmRepository.GetByTypeAsync(filter.Id);
            return farms.Select(x => MapEntityToDto(x)).ToList();
        }

        private FarmResult MapEntityToDto(Farm farm)
        {
            return new FarmResult
            {
                Id = farm.Id,
                Name = farm.Name,
                Address = farm.Address,
                Description = farm.Description,
                CreatedDate = farm.CreatedDate,
                CreatedById = farm.CreatedById,
                UpdatedById = farm.UpdatedById,
                UpdatedDate = farm.UpdatedDate,
                FarmTypeId = farm.FarmTypeId,
                FarmTypeName = farm.FarmType.Name,
                StatusId = farm.StatusId,
            };
        }

        public async Task<FarmResult> FindDetailAsync(IdRequestFilter<long> filter)
        {
            var existing = await _farmRepository.FindAsync(filter.Id);
            if (existing == null)
            {
                return null;
            }

            if ((existing.StatusId == _deletedStatus && !filter.CanGetDeleted) || (existing.StatusId == _inactivedStatus && !filter.CanGetInactived))
            {
                return null;
            }

            var result = MapEntityToDto(existing);
            var pictures = await _farmPictureAppService.GetListByFarmIdAsync(new IdRequestFilter<long>
            {
                Id = filter.Id,
                CanGetDeleted = filter.CanGetDeleted,
                CanGetInactived = filter.CanGetInactived
            });
            result.Pictures = pictures.Select(x => new PictureResult
            {
                Id = x.PictureId
            });

            var createdByUserName = (await _userRepository.FindByIdAsync(existing.CreatedById)).DisplayName;
            result.CreatedBy = createdByUserName;

            var updatedByUserName = (await _userRepository.FindByIdAsync(existing.UpdatedById)).DisplayName;
            result.UpdatedBy = updatedByUserName;

            return result;
        }

        public async Task<IList<FarmResult>> SelectAsync(SelectFilter filter, int page, int pageSize)
        {
            if (filter.Keyword == null)
            {
                filter.Keyword = string.Empty;
            }

            filter.Keyword = filter.Keyword.ToLower();
            var query = _farmEntityRepository.Get(x => x.StatusId != FarmStatuses.Deleted.GetCode())
                .Select(c => new FarmResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CreatedById = c.CreatedById
                });

            if (filter.CreatedById > 0)
            {
                query = query.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.CurrentIds != null && filter.CurrentIds.Any())
            {
                query = query.Where(x => !filter.CurrentIds.Contains(x.Id));
            }

            filter.Keyword = filter.Keyword.ToLower();
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(x => x.Name.ToLower().Contains(filter.Keyword) || x.Description.ToLower().Contains(filter.Keyword));
            }

            if (pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var farms = await query
                .Select(x => new FarmResult()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return farms;
        }

        public async Task<BasePageList<FarmResult>> GetAsync(FarmFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var farmQuery = _farmEntityRepository.Get(x => (x.StatusId == _deletedStatus && filter.CanGetDeleted)
                                    || (x.StatusId == _inactivedStatus && filter.CanGetInactived)
                                    || (x.StatusId != _deletedStatus && x.StatusId != _inactivedStatus));
            if (!string.IsNullOrEmpty(search))
            {
                farmQuery = farmQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            if (filter.ExclusiveUserId.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedById != filter.ExclusiveUserId);
            }

            if (filter.StatusId.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.StatusId == filter.StatusId);
            }

            if (filter.CreatedById.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.UpdatedById.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.UpdatedById == filter.UpdatedById);
            }

            if (filter.FarmTypeId.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.FarmTypeId == filter.FarmTypeId);
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTime.UtcNow);
            }

            var filteredNumber = farmQuery.Select(x => x.Id).Count();

            var query = from farm in farmQuery
                        select new FarmResult
                        {
                            Id = farm.Id,
                            Name = farm.Name,
                            Address = farm.Address,
                            CreatedById = farm.CreatedById,
                            CreatedDate = farm.CreatedDate,
                            Description = farm.Description,
                            UpdatedById = farm.UpdatedById,
                            UpdatedDate = farm.UpdatedDate,
                            StatusId = farm.StatusId
                        };

            var farms = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            await PopulateDetailsAsync(farms, filter);
            var result = new BasePageList<FarmResult>(farms)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task PopulateDetailsAsync(IList<FarmResult> farms, FarmFilter filter)
        {
            var createdByIds = farms.Select(x => x.CreatedById).ToArray();
            var updatedByIds = farms.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = await _userRepository.GetByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetByIdsAsync(updatedByIds);

            var farmIds = farms.Select(x => x.Id);
            var pictures = await _farmPictureAppService.GetListByFarmIdsAsync(farmIds, new IdRequestFilter<long>
            {
                CanGetDeleted = filter.CanGetDeleted,
                CanGetInactived = filter.CanGetInactived
            }, FarmPictureTypes.Thumbnail);

            var userAvatars = await _userPhotoRepository.GetListByUserIdsAsync(createdByIds, UserPictureTypes.Avatar);
            foreach (var farm in farms)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == farm.CreatedById);
                farm.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == farm.UpdatedById);
                farm.UpdatedBy = updatedBy.DisplayName;

                var farmPictures = pictures.Where(x => x.FarmId == farm.Id);
                if (farmPictures != null && farmPictures.Any())
                {
                    farm.Pictures = farmPictures.Select(x => new PictureResult
                    {
                        Id = x.PictureId
                    });
                }

                var avatar = userAvatars.FirstOrDefault(x => x.UserId == farm.CreatedById);
                if (avatar != null)
                {
                    farm.CreatedByPhotoId = avatar.Id;
                }
            }
        }
        #endregion

        #region CRUD
        public async Task<long> CreateAsync(FarmModifyRequest request)
        {
            var id = await _farmRepository.CreateAsync(new Farm()
            {
                FarmTypeId = request.FarmTypeId,
                Name = request.Name,
                Address = request.Address,
                UpdatedById = request.UpdatedById,
                CreatedById = request.CreatedById,
                Description = request.Description,
                StatusId = FarmStatuses.Pending.GetCode()
            });
            if (id > 0 && request.Pictures.Any())
            {
                await _farmPictureAppService.CreateAsync(new FarmPicturesModifyRequest
                {
                    CreatedById = request.CreatedById,
                    CreatedDate = request.CreatedDate,
                    FarmId = id,
                    Pictures = request.Pictures,
                    UpdatedById = request.UpdatedById,
                    UpdatedDate = request.UpdatedDate
                }, needSaveChanges: true);
            }

            return id;
        }

        public async Task<bool> UpdateAsync(FarmModifyRequest request)
        {
            var modifiedDate = DateTime.UtcNow;
            var farm = await _farmRepository.FindAsync(request.Id);
            farm.Description = request.Description;
            farm.Name = request.Name;
            farm.FarmTypeId = request.FarmTypeId;
            farm.UpdatedById = request.UpdatedById;
            farm.UpdatedDate = modifiedDate;
            farm.Address = request.Address;
            var isUpdated = await _farmRepository.UpdateAsync(farm);
            if (isUpdated)
            {
                await _farmPictureAppService.UpdateAsync(new FarmPicturesModifyRequest
                {
                    FarmId = request.Id,
                    CreatedById = request.CreatedById,
                    UpdatedById = request.UpdatedById,
                    CreatedDate = modifiedDate,
                    UpdatedDate = modifiedDate,
                    Pictures = request.Pictures
                }, true);
            }
            return isUpdated;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            // Delete farm products
            await DeleteProductByFarmIdAsync(id);

            // Delete farm pictures
            await _farmPictureAppService.DeleteByFarmIdAsync(id);
            return await _farmRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Delete products by farm id
        /// </summary>
        /// <param name="farmId"></param>
        /// <returns></returns>
        private async Task DeleteProductByFarmIdAsync(long farmId)
        {
            var farmProducts = _farmProductEntityRepository.Get(x => x.FarmId == farmId);
            var productIds = farmProducts.Select(x => x.ProductId).ToList();

            await _productPictureDomainService.DeleteByProductIdsAsync(productIds);

            await _farmProductEntityRepository.DeleteAsync(farmProducts);

            await _productPriceEntityRepository.DeleteAsync(x => productIds.Contains(x.ProductId));

            await _productCategoryRelationEntityRepository
                .DeleteAsync(x => productIds.Contains(x.ProductId));

            await _productEntityRepository.DeleteAsync(x => productIds.Contains(x.Id));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> SoftDeleteAsync(FarmModifyRequest request)
        {
            // Delete farm products
            await UpdateProductStatusByFarmIdAsync(request, ProductStatuses.Deleted);

            // Soft delete farm pictures
            await _farmPictureAppService.UpdateStatusByFarmIdAsync(request.Id, request.UpdatedById, PictureStatuses.Deleted);
            var existing = await _farmRepository.FindAsync(request.Id);
            existing.StatusId = _deletedStatus;
            existing.UpdatedById = request.UpdatedById;
            return await _farmRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeactivateAsync(FarmModifyRequest request)
        {
            await UpdateProductStatusByFarmIdAsync(request, ProductStatuses.Inactived);
            await _farmPictureAppService.UpdateStatusByFarmIdAsync(request.Id, request.UpdatedById, PictureStatuses.Inactived);
            var existing = await _farmRepository.FindAsync(request.Id);
            existing.StatusId = _inactivedStatus;
            existing.UpdatedById = request.UpdatedById;
            return await _farmRepository.UpdateAsync(existing);
        }

        public async Task<bool> ActivateAsync(FarmModifyRequest request)
        {
            await _farmPictureAppService.UpdateStatusByFarmIdAsync(request.Id, request.UpdatedById, PictureStatuses.Actived);
            var existing = await _farmRepository.FindAsync(request.Id);
            existing.StatusId = FarmStatuses.Actived.GetCode();
            existing.UpdatedById = request.UpdatedById;
            return await _farmRepository.UpdateAsync(existing);
        }

        private async Task UpdateProductStatusByFarmIdAsync(FarmModifyRequest request, ProductStatuses productStatus)
        {
            var productIds = _farmProductEntityRepository.Get(x => x.FarmId == request.Id)
                .Select(x => x.ProductId);

            var pictureStatus = productStatus switch
            {
                ProductStatuses.Pending => PictureStatuses.Pending,
                ProductStatuses.Actived => PictureStatuses.Actived,
                ProductStatuses.Reported => PictureStatuses.Reported,
                ProductStatuses.Deleted => PictureStatuses.Deleted,
                _ => PictureStatuses.Inactived,
            };
            await _productPictureDomainService.UpdateStatusByProductIdsAsync(productIds, request.UpdatedById, pictureStatus);

            var existingProducts = await _productEntityRepository.GetAsync(x => productIds.Contains(x.Id));
            foreach (var product in existingProducts)
            {
                product.StatusId = productStatus.GetCode();
                product.UpdatedById = request.UpdatedById;
                product.UpdatedDate = DateTime.UtcNow;
            }
            await _productEntityRepository.UpdateAsync(existingProducts);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region farm pictures
        public async Task<BasePageList<FarmPictureResult>> GetPicturesAsync(FarmPictureFilter filter)
        {
            var farmPicturePageList = await _farmPictureAppService.GetAsync(filter);

            var createdByIds = farmPicturePageList.Collections.GroupBy(x => x.PictureCreatedById).Select(x => x.Key);
            var createdByUsers = await _userRepository.GetByIdsAsync(createdByIds);

            foreach (var farmPicture in farmPicturePageList.Collections)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == farmPicture.PictureCreatedById);
                farmPicture.PictureCreatedBy = createdBy.DisplayName;
            }

            return farmPicturePageList;
        }
        #endregion

        #region farm status
        public IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "")
        {
            search = search != null ? search.ToLower() : "";
            var result = new List<SelectOption>();
            if (filter.Id.HasValue)
            {
                var selected = (FarmStatuses)filter.Id;
                result = SelectOptionUtils.ToSelectOptions(selected).ToList();
            }
            else
            {
                result = SelectOptionUtils.ToSelectOptions<FarmStatuses>().ToList();
            }

            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
            return result;
        }
        #endregion
    }
}
