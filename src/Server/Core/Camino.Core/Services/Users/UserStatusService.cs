using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Identifiers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Core.Contracts.Services.Users;
using Camino.Shared.Requests.Identifiers;
using Camino.Core.Contracts.Repositories.Users;

namespace Camino.Services.Users
{
    public class UserStatusService : IUserStatusService
    {
        private readonly IUserStatusRepository _statusRepository;
        public UserStatusService(IUserStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public IList<UserStatusResult> Search(BaseFilter filter)
        {
            return _statusRepository.Search(filter);
        }

        public async Task<BasePageList<UserStatusResult>> GetAsync(UserStatusFilter filter)
        {
            return await _statusRepository.GetAsync(filter);
        }

        public UserStatusResult Find(int id)
        {
            return _statusRepository.Find(id);
        }

        public UserStatusResult FindByName(string name)
        {
            return _statusRepository.FindByName(name);
        }

        public async Task<int> CreateAsync(UserStatusModifyRequest request)
        {
            return await _statusRepository.CreateAsync(request);
        }

        public async Task<bool> UpdateAsync(UserStatusModifyRequest request)
        {
            return await _statusRepository.UpdateAsync(request);
        }
    }
}
