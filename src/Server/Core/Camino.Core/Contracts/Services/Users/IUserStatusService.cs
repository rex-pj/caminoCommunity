using Camino.Shared.Requests.Filters;
using Camino.Shared.Requests.Identifiers;
using Camino.Shared.Results.Identifiers;
using Camino.Shared.Results.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Services.Users
{
    public interface IUserStatusService
    {
        IList<UserStatusResult> Search(string query = "", int page = 1, int pageSize = 10);
        Task<BasePageList<UserStatusResult>> GetAsync(UserStatusFilter filter);
        UserStatusResult Find(int id);
        UserStatusResult FindByName(string name);
        Task<int> CreateAsync(UserStatusModifyRequest request);
        Task<bool> UpdateAsync(UserStatusModifyRequest request);
    }
}
