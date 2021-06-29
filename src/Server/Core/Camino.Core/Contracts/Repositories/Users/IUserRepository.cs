using Camino.Shared.Results.Identifiers;
using Camino.Shared.Requests.Filters;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Shared.Results.PageList;
using Camino.Shared.Requests.Authentication;
using Camino.Shared.Requests.UpdateItems;
using Camino.Shared.Requests.Identifiers;

namespace Camino.Core.Contracts.Repositories.Users
{
    public interface IUserRepository
    {
        Task<long> CreateAsync(UserModifyRequest request);
        Task<UserResult> FindByEmailAsync(string email);
        Task<UserResult> FindByUsernameAsync(string username);
        Task<IList<UserResult>> GetNameByIdsAsync(IEnumerable<long> ids);
        Task<bool> SoftDeleteAsync(UserModifyRequest request);
        Task<bool> DeactivateAsync(UserModifyRequest request);
        Task<bool> ActiveAsync(UserModifyRequest request);
        Task<bool> ConfirmAsync(UserModifyRequest request);
        Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync(UserIdentifierUpdateRequest request);
        Task<UserResult> FindByIdAsync(long id);
        Task<UserFullResult> FindFullByIdAsync(IdRequestFilter<long> filter);
        Task<UpdateItemRequest> UpdateInfoItemAsync(UpdateItemRequest request);
        Task<bool> UpdateAsync(UserModifyRequest request);
        Task<BasePageList<UserFullResult>> GetAsync(UserFilter filter);
        List<UserFullResult> Search(string query = "", List<long> currentUserIds = null, int page = 1, int pageSize = 10);
    }
}
