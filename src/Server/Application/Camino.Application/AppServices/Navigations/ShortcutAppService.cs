using Camino.Core.Contracts.Repositories.Navigations;
using Camino.Shared.Enums;
using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Navigations.Dtos;
using Camino.Core.Domains.Navigations;
using Camino.Application.Contracts.AppServices.Navigations;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains;
using Microsoft.EntityFrameworkCore;
using Camino.Shared.Utils;
using Camino.Application.Contracts.Utils;

namespace Camino.Core.Core.Services.Navigations
{
    public class ShortcutAppService : IShortcutAppService, IScopedDependency
    {
        private readonly IShortcutRepository _shortcutRepository;
        private readonly IEntityRepository<Shortcut> _shortcutEntityRepository;
        private readonly int _inactivedStatus;
        private readonly int _activedStatus;

        public ShortcutAppService(IShortcutRepository shortcutRepository, IEntityRepository<Shortcut> shortcutEntityRepository)
        {
            _shortcutRepository = shortcutRepository;
            _inactivedStatus = (int)ShortcutStatuses.Inactived;
            _activedStatus = (int)ShortcutStatuses.Actived;
            _shortcutEntityRepository = shortcutEntityRepository;
        }

        #region get
        public async Task<ShortcutResult> FindAsync(IdRequestFilter<int> filter)
        {
            var existing = await _shortcutRepository.FindAsync(filter.Id);
            if (existing == null)
            {
                return null;
            }

            if (!filter.CanGetInactived && existing.StatusId == _inactivedStatus)
            {
                return null;
            }

            return MapEntityToDto(existing);
        }

        public async Task<ShortcutResult> FindByNameAsync(string name)
        {
            var existing = await _shortcutRepository.FindByNameAsync(name);
            if (existing == null)
            {
                return null;
            }

            return MapEntityToDto(existing);
        }

        public async Task<BasePageList<ShortcutResult>> GetAsync(ShortcutFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var shortcutQuery = _shortcutEntityRepository.Get(x => filter.CanGetInactived || x.StatusId != _inactivedStatus);
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
        #endregion

        #region CRUD
        public async Task<int> CreateAsync(ShortcutModifyRequest request)
        {
            var newShortcut = new Shortcut()
            {
                Description = request.Description,
                Name = request.Name,
                Icon = request.Icon,
                TypeId = request.TypeId,
                Url = request.Url,
                DisplayOrder = request.Order,
                CreatedById = request.CreatedById,
                CreatedDate = DateTime.UtcNow,
                UpdatedById = request.UpdatedById,
                UpdatedDate = DateTime.UtcNow,
                StatusId = ShortcutStatuses.Actived.GetCode(),
            };

            return await _shortcutRepository.CreateAsync(newShortcut);
        }

        public async Task<bool> UpdateAsync(ShortcutModifyRequest request)
        {
            var existing = await _shortcutRepository.FindAsync(request.Id);
            existing.Description = request.Description;
            existing.Name = request.Name;
            existing.Icon = request.Icon;
            existing.TypeId = request.TypeId;
            existing.Url = request.Url;
            existing.DisplayOrder = request.Order;
            existing.UpdatedById = request.UpdatedById;
            return await _shortcutRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeactivateAsync(int id, long updatedById)
        {
            var existing = await _shortcutRepository.FindAsync(id);
            existing.StatusId = _inactivedStatus;
            existing.UpdatedById = updatedById;
            return await _shortcutRepository.UpdateAsync(existing);
        }

        public async Task<bool> ActiveAsync(int id, long updatedById)
        {
            var existing = await _shortcutRepository.FindAsync(id);
            existing.StatusId = _activedStatus;
            existing.UpdatedById = updatedById;
            return await _shortcutRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _shortcutRepository.DeleteAsync(id);
        }
        #endregion

        private ShortcutResult MapEntityToDto(Shortcut entity)
        {
            return new ShortcutResult
            {
                Description = entity.Description,
                Id = entity.Id,
                Name = entity.Name,
                Icon = entity.Icon,
                TypeId = entity.TypeId,
                Url = entity.Url,
                DisplayOrder = entity.DisplayOrder,
                StatusId = entity.StatusId,
                CreatedDate = entity.CreatedDate,
                CreatedById = entity.CreatedById,
                UpdatedDate = entity.UpdatedDate,
                UpdatedById = entity.UpdatedById
            };
        }

        #region shortcut type
        public IList<SelectOption> GetShortcutTypes(ShortcutTypeFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var result = new List<SelectOption>();
            if (filter.Id > 0)
            {
                var selected = (ShortcutTypes)filter.Id;
                result = SelectOptionUtils.ToSelectOptions(selected).ToList();
            }
            else
            {
                result = SelectOptionUtils.ToSelectOptions<ShortcutTypes>().ToList();
            }

            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
            return result;
        }
        #endregion

        #region shortcut status
        public IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "")
        {
            search = search != null ? search.ToLower() : "";
            var result = new List<SelectOption>();
            if (filter.Id.HasValue)
            {
                var selected = (ShortcutStatuses)filter.Id;
                result = SelectOptionUtils.ToSelectOptions(selected).ToList();
            }
            else
            {
                result = SelectOptionUtils.ToSelectOptions<ShortcutStatuses>().ToList();
            }

            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
            return result;
        }
        #endregion
    }
}
