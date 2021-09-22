using Camino.Shared.Results.Identifiers;
using Camino.Shared.Requests.Filters;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Shared.Results.PageList;
using Camino.Shared.Requests.Authentication;
using Camino.Shared.Requests.UpdateItems;
using Camino.Shared.Requests.Identifiers;

namespace Camino.Core.Contracts.Services.Users
{
    public interface IUserService
    {
        Task<long> CreateAsync(UserModifyRequest request);
        Task<UserResult> FindByEmailAsync(string email);
        Task<UserResult> FindByUsernameAsync(string username);
        Task<bool> SoftDeleteAsync(UserModifyRequest request);
        Task<bool> DeactivateAsync(UserModifyRequest request);
        Task<bool> ActiveAsync(UserModifyRequest request);
        Task<bool> ConfirmAsync(UserModifyRequest request);
        Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync(UserIdentifierUpdateRequest request);
        Task<UserResult> FindByIdAsync(long id);
        Task<UserFullResult> FindFullByIdAsync(IdRequestFilter<long> filter);
        Task<PartialUpdateRequest> PartialUpdateAsync(PartialUpdateRequest request);
        Task<bool> UpdateAsync(UserModifyRequest request);
        Task<BasePageList<UserFullResult>> GetAsync(UserFilter filter);
        Task<List<UserFullResult>> SearchAsync(UserFilter filter, List<long> currentUserIds = null);
    }
}
