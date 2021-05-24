using Camino.Core.Contracts.Data;
using Camino.Core.Domain.Navigations;
using System.Threading.Tasks;
using System.Linq;
using Camino.Shared.Results.Navigations;
using LinqToDB;
using Camino.Shared.Results.PageList;
using Camino.Shared.Requests.Filters;
using System;
using System.Collections.Generic;
using Camino.Shared.General;
using Camino.Infrastructure.Enums;
using Camino.Core.Utils;
using Camino.Core.Contracts.Repositories.Navigations;

namespace Camino.Infrastructure.Repositories.Navigations
{
    public class ShortcutRepository : IShortcutRepository
    {
        private readonly IRepository<Shortcut> _shortcutRepository;

        public ShortcutRepository(IRepository<Shortcut> shortcutRepository)
        {
            _shortcutRepository = shortcutRepository;
        }

        public async Task<ShortcutResult> FindAsync(int id)
        {
            var shortcut = await (_shortcutRepository.Get(x => x.Id == id)
                .Select(x => new ShortcutResult
                {
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Name,
                    Icon = x.Icon,
                    TypeId = x.TypeId,
                    Url = x.Url,
                    Order = x.Order
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
                   Order = x.Order
               })).FirstOrDefaultAsync();

            return shortcut;
        }

        public async Task<BasePageList<ShortcutResult>> GetAsync(ShortcutFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var shortcutQuery = _shortcutRepository.Table;
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
                Order = x.Order
            });

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

        public IList<SelectOption> GetShortcutTypes(ShortcutTypeFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var result = new List<SelectOption>();
            if (filter.ShortcutTypeId > 0)
            {
                var selected = (ShortcutType)filter.ShortcutTypeId;
                result = EnumUtil.ToSelectOptions(selected).ToList();
            }
            else
            {
                result = EnumUtil.ToSelectOptions<ShortcutType>().ToList();
            }

            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
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
                Order = request.Order
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
                .UpdateAsync();

            return true;
        }
    }
}
