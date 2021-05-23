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
    public class ShortcutService : IShortcutService
    {
        private readonly IShortcutRepository _shortcutRepository;
        public ShortcutService(IShortcutRepository shortcutRepository)
        {
            _shortcutRepository = shortcutRepository;
        }

        public async Task<ShortcutResult> FindAsync(int id)
        {
            return await _shortcutRepository.FindAsync(id);
        }

        public async Task<ShortcutResult> FindByNameAsync(string name)
        {
            return await _shortcutRepository.FindByNameAsync(name);
        }

        public async Task<BasePageList<ShortcutResult>> GetAsync(ShortcutFilter filter)
        {
            return await _shortcutRepository.GetAsync(filter);
        }

        public IList<SelectOption> GetShortcutTypes(ShortcutTypeFilter filter)
        {
            return _shortcutRepository.GetShortcutTypes(filter);
        }

        public async Task<int> CreateAsync(ShortcutModifyRequest request)
        {
            return await _shortcutRepository.CreateAsync(request);
        }

        public async Task<bool> UpdateAsync(ShortcutModifyRequest request)
        {
            return await _shortcutRepository.UpdateAsync(request);
        }
    }
}
