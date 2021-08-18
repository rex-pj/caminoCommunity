using Camino.Core.Contracts.Data;
using Camino.Core.Domain.Navigations;
using System.Threading.Tasks;
using System.Linq;
using Camino.Shared.Results.Navigations;
using LinqToDB;
using Camino.Shared.Results.PageList;
using Camino.Shared.Requests.Filters;
using System;
using Camino.Core.Utils;
using Camino.Core.Contracts.Repositories.Navigations;
using Camino.Shared.Enums;

namespace Camino.Infrastructure.Repositories.Navigations
{
    public class ShortcutRepository : IShortcutRepository
    {
        private readonly IRepository<Shortcut> _shortcutRepository;

        public ShortcutRepository(IRepository<Shortcut> shortcutRepository)
        {
            _shortcutRepository = shortcutRepository;
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
                    Order = x.Order,
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
                   Order = x.Order,
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
            var search = filter.Search != null ? filter.Search.ToLower() : "";
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
                Order = x.Order,
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
                .OrderBy(x => x.Order)
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
                Order = request.Order,
                CreatedById = request.CreatedById,
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedById = request.UpdatedById,
                UpdatedDate = DateTimeOffset.UtcNow,
                StatusId = ShortcutStatus.Actived.GetCode(),
            };

            var id = await _shortcutRepository.AddWithInt32EntityAsync(newCategory);
            return id;
        }

        public async Task<bool> UpdateAsync(ShortcutModifyRequest request)
        {
            var exist = await _shortcutRepository.Get(x => x.Id == request.Id)
                .Set(x => x.Description, request.Description)
                .Set(x => x.Name, request.Name)
                .Set(x => x.Icon, request.Icon)
                .Set(x => x.TypeId, request.TypeId)
                .Set(x => x.Url, request.Url)
                .Set(x => x.Order, request.Order)
                .Set(x => x.UpdatedById, request.UpdatedById)
                .Set(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> DeactivateAsync(ShortcutModifyRequest request)
        {
            await _shortcutRepository.Get(x => x.Id == request.Id)
                .Set(x => x.StatusId, (int)ProductCategoryStatus.Inactived)
                .Set(x => x.UpdatedById, request.UpdatedById)
                .Set(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> ActiveAsync(ShortcutModifyRequest request)
        {
            await _shortcutRepository.Get(x => x.Id == request.Id)
                .Set(x => x.StatusId, (int)ProductCategoryStatus.Actived)
                .Set(x => x.UpdatedById, request.UpdatedById)
                .Set(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedNumbers = await _shortcutRepository.Get(x => x.Id == id).DeleteAsync();
            return deletedNumbers > 0;
        }
    }
}
