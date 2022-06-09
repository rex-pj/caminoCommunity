using Camino.Application.Contracts.AppServices.Users.Dtos;

namespace Camino.Application.Contracts.AppServices.Users
{
    public interface IUserAppService
    {
        Task<bool> ActiveAsync(long id, long updatedById);
        Task<bool> ConfirmAsync(long id, long updatedById);
        Task<long> CreateAsync(UserModifyRequest request);
        Task<bool> DeactivateAsync(long id, long updatedById);
        Task<UserResult> FindByEmailAsync(string email);
        Task<UserResult> FindByIdAsync(long id);
        Task<UserResult> FindByUsernameAsync(string username);
        Task<UserFullResult> FindFullByIdAsync(IdRequestFilter<long> filter);
        Task<BasePageList<UserFullResult>> GetAsync(UserFilter filter);
        Task<PartialUpdateRequest> PartialUpdateAsync(PartialUpdateRequest request);
        Task<List<UserFullResult>> SearchAsync(UserFilter filter, List<long> currentUserIds = null);
        Task<bool> SoftDeleteAsync(long id, long updatedById);
        Task<bool> UpdateAsync(UserModifyRequest request);
        Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync(UserIdentifierUpdateRequest request);
    }
}
