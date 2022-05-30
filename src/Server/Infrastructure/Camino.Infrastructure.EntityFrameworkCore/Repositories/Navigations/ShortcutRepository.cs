﻿using Camino.Core.Contracts.Data;
using Camino.Core.Domain.Navigations;
using System.Threading.Tasks;
using System.Linq;
using Camino.Shared.Results.Navigations;
using Camino.Shared.Results.PageList;
using Camino.Shared.Requests.Filters;
using System;
using Camino.Core.Utils;
using Camino.Core.Contracts.Repositories.Navigations;
using Camino.Shared.Enums;
using Camino.Core.Contracts.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Camino.Infrastructure.EntityFrameworkCore.Extensions;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Navigations
{
    public class ShortcutRepository : IShortcutRepository, IScopedDependency
    {
        private readonly IEntityRepository<Shortcut> _shortcutRepository;
        private readonly IAppDbContext _dbContext;

        public ShortcutRepository(IEntityRepository<Shortcut> shortcutRepository, IAppDbContext dbContext)
        {
            _shortcutRepository = shortcutRepository;
            _dbContext = dbContext;
        }

        public async Task<ShortcutResult> FindAsync(IdRequestFilter<int> filter)
        {
            var inactivedStatus = ShortcutStatus.Inactived.GetCode();
            var shortcut = await (_shortcutRepository.Get(x => x.Id == filter.Id && (filter.CanGetInactived || x.StatusId != inactivedStatus))
                .Select(x => new ShortcutResult
                {
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Name,
                    Icon = x.Icon,
                    TypeId = x.TypeId,
                    Url = x.Url,
                    DisplayOrder = x.DisplayOrder,
                    StatusId = x.StatusId,
                    CreatedDate = x.CreatedDate,
                    CreatedById = x.CreatedById,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedById = x.UpdatedById
                })).FirstOrDefaultAsync();

            return shortcut;
        }

        public async Task<ShortcutResult> FindByNameAsync(string name)
        {
            var shortcut = await (_shortcutRepository.Get(x => x.Name == name)
               .Select(x => new ShortcutResult
               {
                   Description = x.Description,
                   Id = x.Id,
                   Name = x.Name,
                   Icon = x.Icon,
                   TypeId = x.TypeId,
                   Url = x.Url,
                   DisplayOrder = x.DisplayOrder,
                   StatusId = x.StatusId,
                   CreatedDate = x.CreatedDate,
                   CreatedById = x.CreatedById,
                   UpdatedDate = x.UpdatedDate,
                   UpdatedById = x.UpdatedById
               })).FirstOrDefaultAsync();

            return shortcut;
        }

        public async Task<BasePageList<ShortcutResult>> GetAsync(ShortcutFilter filter)
        {
            var inactivedStatus = ShortcutStatus.Inactived.GetCode();
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var shortcutQuery = _shortcutRepository.Get(x => filter.CanGetInactived || x.StatusId != inactivedStatus);
            if (!string.IsNullOrEmpty(search))
            {
                shortcutQuery = shortcutQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            var query = shortcutQuery.Select(x => new ShortcutResult
            {
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                Icon = x.Icon,
                TypeId = x.TypeId,
                Url = x.Url,
                DisplayOrder = x.DisplayOrder,
                StatusId = x.StatusId,
                CreatedDate = x.CreatedDate,
                CreatedById = x.CreatedById,
                UpdatedDate = x.UpdatedDate,
                UpdatedById = x.UpdatedById
            });

            if (filter.StatusId.HasValue)
            {
                query = query.Where(x => x.StatusId == filter.StatusId);
            }

            if (filter.TypeId.HasValue && filter.TypeId != 0)
            {
                query = query.Where(x => x.TypeId == filter.TypeId);
            }

            var filteredNumber = query.Select(x => x.Id).Count();

            var categories = await query
                .OrderBy(x => x.DisplayOrder)
                .Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var result = new BasePageList<ShortcutResult>(categories)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }

        public async Task<int> CreateAsync(ShortcutModifyRequest request)
        {
            var newCategory = new Shortcut()
            {
                Description = request.Description,
                Name = request.Name,
                Icon = request.Icon,
                TypeId = request.TypeId,
                Url = request.Url,
                DisplayOrder = request.Order,
                CreatedById = request.CreatedById,
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedById = request.UpdatedById,
                UpdatedDate = DateTimeOffset.UtcNow,
                StatusId = ShortcutStatus.Actived.GetCode(),
            };

            await _shortcutRepository.InsertAsync(newCategory);
            await _dbContext.SaveChangesAsync();
            return newCategory.Id;
        }

        public async Task<bool> UpdateAsync(ShortcutModifyRequest request)
        {
            var existing = await _shortcutRepository.FindAsync(x => x.Id == request.Id);
            existing.Description = request.Description;
            existing.Name = request.Name;
            existing.Icon = request.Icon;
            existing.TypeId = request.TypeId;
            existing.Url = request.Url;
            existing.DisplayOrder = request.Order;
            existing.UpdatedById = request.UpdatedById;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeactivateAsync(ShortcutModifyRequest request)
        {
            var existing = await _shortcutRepository.FindAsync(x => x.Id == request.Id);
            existing.StatusId = (int)ProductCategoryStatus.Inactived;
            existing.UpdatedById = request.UpdatedById;
            existing.UpdatedDate = DateTimeOffset.UtcNow;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> ActiveAsync(ShortcutModifyRequest request)
        {
            var existing = await _shortcutRepository.FindAsync(x => x.Id == request.Id);
            existing.StatusId = (int)ProductCategoryStatus.Actived;
            existing.UpdatedById = request.UpdatedById;
            existing.UpdatedDate = DateTimeOffset.UtcNow;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedNumbers = await _shortcutRepository.DeleteAsync(x => x.Id == id);
            await _dbContext.SaveChangesAsync();
            return deletedNumbers > 0;
        }
    }
}
