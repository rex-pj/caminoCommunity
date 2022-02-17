using Camino.Core.Contracts.DependencyInjection;
using Camino.Core.Contracts.Repositories.Navigations;
using Camino.Core.Contracts.Services.Navigations;
using Camino.Shared.General;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Navigations;
using Camino.Shared.Results.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Services.Navigations
{
    public class ShortcutService : IShortcutService, IScopedDependency
    {
        private readonly IShortcutRepository _shortcutRepository;
        private readonly IShortcutTypeRepository _shortcutTypeRepository;
        private readonly IShortcutStatusRepository _shortcutStatusRepository;

        public ShortcutService(IShortcutRepository shortcutRepository,
            IShortcutTypeRepository shortcutTypeRepository,
            IShortcutStatusRepository shortcutStatusRepository)
        {
            _shortcutRepository = shortcutRepository;
            _shortcutTypeRepository = shortcutTypeRepository;
            _shortcutStatusRepository = shortcutStatusRepository;
        }

        #region get
        public async Task<ShortcutResult> FindAsync(IdRequestFilter<int> filter)
        {
            return await _shortcutRepository.FindAsync(filter);
        }

        public async Task<ShortcutResult> FindByNameAsync(string name)
        {
            return await _shortcutRepository.FindByNameAsync(name);
        }

        public async Task<BasePageList<ShortcutResult>> GetAsync(ShortcutFilter filter)
        {
            return await _shortcutRepository.GetAsync(filter);
        }
        #endregion

        #region CRUD
        public async Task<int> CreateAsync(ShortcutModifyRequest request)
        {
            return await _shortcutRepository.CreateAsync(request);
        }

        public async Task<bool> UpdateAsync(ShortcutModifyRequest request)
        {
            return await _shortcutRepository.UpdateAsync(request);
        }

        public async Task<bool> DeactivateAsync(ShortcutModifyRequest farmType)
        {
            return await _shortcutRepository.DeactivateAsync(farmType);
        }

        public async Task<bool> ActiveAsync(ShortcutModifyRequest farmType)
        {
            return await _shortcutRepository.ActiveAsync(farmType);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _shortcutRepository.DeleteAsync(id);
        }
        #endregion

        #region shortcut type
        public IList<SelectOption> GetShortcutTypes(ShortcutTypeFilter filter)
        {
            return _shortcutTypeRepository.Get(filter);
        }
        #endregion

        #region shortcut status
        public IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "")
        {
            return _shortcutStatusRepository.Search(filter, search);
        }
        #endregion
    }
}
