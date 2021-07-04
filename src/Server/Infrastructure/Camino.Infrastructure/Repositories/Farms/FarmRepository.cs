using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Farms;
using Camino.Shared.Results.Farms;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Domain.Products;
using Camino.Core.Domain.Farms;
using Camino.Shared.Requests.Farms;
using Camino.Core.Contracts.Repositories.Products;
using LinqToDB.Tools;
using Camino.Shared.Enums;
using Camino.Core.Utils;

namespace Camino.Infrastructure.Repositories.Farms
{
    public class FarmRepository : IFarmRepository
    {
        private readonly IRepository<Farm> _farmRepository;
        private readonly IRepository<FarmType> _farmTypeRepository;
        private readonly IRepository<FarmProduct> _farmProductRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductPrice> _productPriceRepository;
        private readonly IRepository<ProductCategoryRelation> _productCategoryRelationRepository;
        private readonly IProductPictureRepository _productPictureRepository;

        public FarmRepository(IRepository<Farm> farmRepository, IRepository<FarmType> farmTypeRepository,
            IRepository<FarmProduct> farmProductRepository, IRepository<Product> productRepository,
            IRepository<ProductPrice> productPriceRepository, IRepository<ProductCategoryRelation> productCategoryRelationRepository,
            IProductPictureRepository productPictureRepository)
        {
            _farmRepository = farmRepository;
            _farmTypeRepository = farmTypeRepository;
            _farmProductRepository = farmProductRepository;
            _productRepository = productRepository;
            _productPriceRepository = productPriceRepository;
            _productCategoryRelationRepository = productCategoryRelationRepository;
            _productPictureRepository = productPictureRepository;
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

        public async Task<IList<FarmResult>> SelectAsync(SelectFilter filter, int page = 1, int pageSize = 10)
        {
            if (filter.Search == null)
            {
                filter.Search = string.Empty;
            }

            filter.Search = filter.Search.ToLower();
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
                query = query.Where(x => x.Id.NotIn(filter.CurrentIds));
            }

            filter.Search = filter.Search.ToLower();
            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(filter.Search) || x.Description.ToLower().Contains(filter.Search));
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
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var farmQuery = _farmRepository.Get(x => (x.StatusId == deletedStatus && filter.CanGetDeleted)
                                    || (x.StatusId == inactivedStatus && filter.CanGetInactived)
                                    || (x.StatusId != deletedStatus && x.StatusId != inactivedStatus));
            if (!string.IsNullOrEmpty(search))
            {
                farmQuery = farmQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            if (filter.ExclusiveCreatedById.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedById != filter.ExclusiveCreatedById);
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

            return await _farmRepository.AddWithInt64EntityAsync(newFarm);
        }

        public async Task<bool> UpdateAsync(FarmModifyRequest request)
        {
            var updatedDate = DateTimeOffset.UtcNow;
            var farm = _farmRepository.FirstOrDefault(x => x.Id == request.Id);
            farm.Description = request.Description;
            farm.Name = request.Name;
            farm.FarmTypeId = request.FarmTypeId;
            farm.UpdatedById = request.UpdatedById;
            farm.UpdatedDate = updatedDate;
            farm.Address = request.Address;

            await _farmRepository.UpdateAsync(farm);

            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            // Delete farm products
            await DeleteProductByFarmIdAsync(id);

            // Delete farm
            await _farmRepository.Get(x => x.Id == id)
                .DeleteAsync();

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

            await farmProducts.DeleteAsync();

            await _productPriceRepository.Get(x => x.ProductId.In(productIds))
                .DeleteAsync();

            await _productCategoryRelationRepository
                .Get(x => x.ProductId.In(productIds))
                .DeleteAsync();

            await _productRepository.Get(x => x.Id.In(productIds))
                .DeleteAsync();
        }

        public async Task<bool> SoftDeleteAsync(FarmModifyRequest request)
        {
            // Delete farm products
            await UpdateProductStatusByFarmIdAsync(request, ProductStatus.Deleted);

            // Delete farm
            await _farmRepository.Get(x => x.Id == request.Id)
                .Set(x => x.StatusId, (int)FarmStatus.Deleted)
                .Set(x => x.UpdatedById, request.UpdatedById)
                .Set(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
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

            await _productRepository.Get(x => x.Id.In(productIds))
                .Set(x => x.StatusId, productStatus.GetCode())
                .Set(x => x.UpdatedById, request.UpdatedById)
                .Set(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();
        }

        public async Task<bool> DeactivateAsync(FarmModifyRequest request)
        {
            await UpdateProductStatusByFarmIdAsync(request, ProductStatus.Inactived);

            await _farmRepository.Get(x => x.Id == request.Id)
                .Set(x => x.StatusId, FarmStatus.Inactived.GetCode())
                .Set(x => x.UpdatedById, request.UpdatedById)
                .Set(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> ActiveAsync(FarmModifyRequest request)
        {
            await _farmRepository.Get(x => x.Id == request.Id)
                .Set(x => x.StatusId, (int)FarmStatus.Actived)
                .Set(x => x.UpdatedById, request.UpdatedById)
                .Set(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }
    }
}
