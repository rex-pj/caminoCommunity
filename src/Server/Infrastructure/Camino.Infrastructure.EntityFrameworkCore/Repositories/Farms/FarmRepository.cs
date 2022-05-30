using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Farms;
using Camino.Shared.Results.Farms;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Domain.Products;
using Camino.Core.Domain.Farms;
using Camino.Shared.Requests.Farms;
using Camino.Core.Contracts.Repositories.Products;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Camino.Core.Contracts.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Camino.Infrastructure.EntityFrameworkCore.Extensions;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Farms
{
    public class FarmRepository : IFarmRepository, IScopedDependency
    {
        private readonly IEntityRepository<Farm> _farmRepository;
        private readonly IEntityRepository<FarmType> _farmTypeRepository;
        private readonly IEntityRepository<FarmProduct> _farmProductRepository;
        private readonly IEntityRepository<Product> _productRepository;
        private readonly IEntityRepository<ProductPrice> _productPriceRepository;
        private readonly IEntityRepository<ProductCategoryRelation> _productCategoryRelationRepository;
        private readonly IProductPictureRepository _productPictureRepository;
        private readonly IAppDbContext _dbContext;

        public FarmRepository(IEntityRepository<Farm> farmRepository, IEntityRepository<FarmType> farmTypeRepository,
            IEntityRepository<FarmProduct> farmProductRepository, IEntityRepository<Product> productRepository,
            IEntityRepository<ProductPrice> productPriceRepository, IEntityRepository<ProductCategoryRelation> productCategoryRelationRepository,
            IProductPictureRepository productPictureRepository, IAppDbContext dbContext)
        {
            _farmRepository = farmRepository;
            _farmTypeRepository = farmTypeRepository;
            _farmProductRepository = farmProductRepository;
            _productRepository = productRepository;
            _productPriceRepository = productPriceRepository;
            _productCategoryRelationRepository = productCategoryRelationRepository;
            _productPictureRepository = productPictureRepository;
            _dbContext = dbContext;
        }

        public async Task<FarmResult> FindAsync(IdRequestFilter<long> filter)
        {
            var deletedStatus = FarmStatus.Deleted.GetCode();
            var inactivedStatus = FarmStatus.Inactived.GetCode();
            var exist = await (from farm in _farmRepository
                               .Get(x => x.Id == filter.Id)
                               join farmType in _farmTypeRepository.Table
                               on farm.FarmTypeId equals farmType.Id
                               where (farm.StatusId == deletedStatus && filter.CanGetDeleted)
                                    || (farm.StatusId == inactivedStatus && filter.CanGetInactived)
                                    || (farm.StatusId != deletedStatus && farm.StatusId != inactivedStatus)
                               select new FarmResult
                               {
                                   CreatedDate = farm.CreatedDate,
                                   CreatedById = farm.CreatedById,
                                   Id = farm.Id,
                                   Name = farm.Name,
                                   Address = farm.Address,
                                   UpdatedById = farm.UpdatedById,
                                   UpdatedDate = farm.UpdatedDate,
                                   Description = farm.Description,
                                   FarmTypeId = farm.FarmTypeId,
                                   FarmTypeName = farmType.Name
                               }).FirstOrDefaultAsync();

            return exist;
        }

        public async Task<FarmResult> FindDetailAsync(IdRequestFilter<long> filter)
        {
            var deletedStatus = FarmStatus.Deleted.GetCode();
            var inactivedStatus = FarmStatus.Inactived.GetCode();
            var exist = await (from farm in _farmRepository
                               .Get(x => x.Id == filter.Id)
                               join farmType in _farmTypeRepository.Table
                               on farm.FarmTypeId equals farmType.Id
                               where (farm.StatusId == deletedStatus && filter.CanGetDeleted)
                                    || (farm.StatusId == inactivedStatus && filter.CanGetInactived)
                                    || (farm.StatusId != deletedStatus && farm.StatusId != inactivedStatus)
                               select new FarmResult
                               {
                                   Id = farm.Id,
                                   Name = farm.Name,
                                   Address = farm.Address,
                                   Description = farm.Description,
                                   CreatedDate = farm.CreatedDate,
                                   CreatedById = farm.CreatedById,
                                   UpdatedById = farm.UpdatedById,
                                   UpdatedDate = farm.UpdatedDate,
                                   FarmTypeName = farmType.Name,
                                   FarmTypeId = farm.FarmTypeId,
                                   StatusId = farm.StatusId,
                               }).FirstOrDefaultAsync();

            return exist;
        }

        public async Task<IList<FarmResult>> SelectAsync(SelectFilter filter, int page, int pageSize)
        {
            if (filter.Keyword == null)
            {
                filter.Keyword = string.Empty;
            }

            filter.Keyword = filter.Keyword.ToLower();
            var query = _farmRepository.Get(x => x.StatusId != FarmStatus.Deleted.GetCode())
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

        public FarmResult FindByName(string name)
        {
            var exist = _farmRepository.Get(x => x.Name == name && x.StatusId != FarmStatus.Deleted.GetCode())
                .Select(x => new FarmResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    FarmTypeId = x.FarmTypeId,
                    UpdatedById = x.UpdatedById,
                    CreatedById = x.CreatedById,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .FirstOrDefault();

            return exist;
        }

        public async Task<BasePageList<FarmResult>> GetAsync(FarmFilter filter)
        {
            var deletedStatus = FarmStatus.Deleted.GetCode();
            var inactivedStatus = FarmStatus.Inactived.GetCode();
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var farmQuery = _farmRepository.Get(x => (x.StatusId == deletedStatus && filter.CanGetDeleted)
                                    || (x.StatusId == inactivedStatus && filter.CanGetInactived)
                                    || (x.StatusId != deletedStatus && x.StatusId != inactivedStatus));
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
                farmQuery = farmQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTimeOffset.UtcNow);
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

            var result = new BasePageList<FarmResult>(farms)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task<IList<FarmResult>> GetFarmByTypeIdAsync(IdRequestFilter<int> typeIdFilter)
        {
            var deletedStatus = FarmStatus.Deleted.GetCode();
            var inactivedStatus = FarmStatus.Inactived.GetCode();
            return await _farmRepository
                .Get(x => x.FarmTypeId == typeIdFilter.Id
                && ((x.StatusId == deletedStatus && typeIdFilter.CanGetDeleted)
                    || (x.StatusId == inactivedStatus && typeIdFilter.CanGetInactived)
                    || (x.StatusId != deletedStatus && x.StatusId != inactivedStatus)))
                .Select(x => new FarmResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreatedById = x.CreatedById,
                    CreatedDate = x.CreatedDate,
                    Description = x.Description,
                    UpdatedById = x.UpdatedById,
                    UpdatedDate = x.UpdatedDate
                })
                .ToListAsync();
        }

        public async Task<long> CreateAsync(FarmModifyRequest request)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            var newFarm = new Farm()
            {
                FarmTypeId = request.FarmTypeId,
                Name = request.Name,
                Address = request.Address,
                UpdatedById = request.UpdatedById,
                CreatedById = request.CreatedById,
                CreatedDate = modifiedDate,
                UpdatedDate = modifiedDate,
                Description = request.Description,
                StatusId = FarmStatus.Pending.GetCode()
            };

            await _farmRepository.InsertAsync(newFarm);
            await _dbContext.SaveChangesAsync();
            return newFarm.Id;
        }

        public async Task<bool> UpdateAsync(FarmModifyRequest request)
        {
            var updatedDate = DateTimeOffset.UtcNow;
            var farm = _farmRepository.Find(x => x.Id == request.Id);
            farm.Description = request.Description;
            farm.Name = request.Name;
            farm.FarmTypeId = request.FarmTypeId;
            farm.UpdatedById = request.UpdatedById;
            farm.UpdatedDate = updatedDate;
            farm.Address = request.Address;

            await _farmRepository.UpdateAsync(farm);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            // Delete farm products
            await DeleteProductByFarmIdAsync(id);

            // Delete farm
            await _farmRepository.DeleteAsync(x => x.Id == id);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Delete products by farm id
        /// </summary>
        /// <param name="farmId"></param>
        /// <returns></returns>
        private async Task DeleteProductByFarmIdAsync(long farmId)
        {
            var farmProducts = _farmProductRepository.Get(x => x.FarmId == farmId);
            var productIds = farmProducts.Select(x => x.ProductId).ToList();

            await _productPictureRepository.DeleteByProductIdsAsync(productIds);

            await _farmProductRepository.DeleteAsync(farmProducts);

            await _productPriceRepository.DeleteAsync(x => productIds.Contains(x.ProductId));

            await _productCategoryRelationRepository
                .DeleteAsync(x => productIds.Contains(x.ProductId));

            await _productRepository.DeleteAsync(x => productIds.Contains(x.Id));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> SoftDeleteAsync(FarmModifyRequest request)
        {
            // Delete farm products
            await UpdateProductStatusByFarmIdAsync(request, ProductStatus.Deleted);

            // Delete farm
            var existing = await _farmRepository.FindAsync(x => x.Id == request.Id);
            existing.StatusId = FarmStatus.Deleted.GetCode();
            existing.UpdatedById = request.UpdatedById;
            existing.UpdatedDate = DateTimeOffset.UtcNow;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        private async Task UpdateProductStatusByFarmIdAsync(FarmModifyRequest request, ProductStatus productStatus)
        {
            var productIds = _farmProductRepository.Get(x => x.FarmId == request.Id)
                .Select(x => x.ProductId);

            var pictureStatus = productStatus switch
            {
                ProductStatus.Pending => PictureStatus.Pending,
                ProductStatus.Actived => PictureStatus.Actived,
                ProductStatus.Reported => PictureStatus.Reported,
                ProductStatus.Deleted => PictureStatus.Deleted,
                _ => PictureStatus.Inactived,
            };
            await _productPictureRepository.UpdateStatusByProductIdsAsync(productIds, request.UpdatedById, pictureStatus);

            var existingProducts = await _productRepository.GetAsync(x => productIds.Contains(x.Id));
            foreach (var product in existingProducts)
            {
                product.StatusId = productStatus.GetCode();
                product.UpdatedById = request.UpdatedById;
                product.UpdatedDate = DateTimeOffset.UtcNow;
            }
            await _productRepository.UpdateAsync(existingProducts);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeactivateAsync(FarmModifyRequest request)
        {
            await UpdateProductStatusByFarmIdAsync(request, ProductStatus.Inactived);

            var existing = await _farmRepository.FindAsync(x => x.Id == request.Id);
                existing.StatusId = FarmStatus.Inactived.GetCode();
            existing.UpdatedById = request.UpdatedById;
            existing.UpdatedDate = DateTimeOffset.UtcNow;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> ActiveAsync(FarmModifyRequest request)
        {
            var existing = await _farmRepository.FindAsync(x => x.Id == request.Id);
            existing.StatusId = FarmStatus.Actived.GetCode();
            existing.UpdatedById = request.UpdatedById;
            existing.UpdatedDate = DateTimeOffset.UtcNow;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
