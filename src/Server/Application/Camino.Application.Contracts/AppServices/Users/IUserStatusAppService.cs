using Camino.Application.Contracts.AppServices.Users.Dtos;

namespace Camino.Application.Contracts.AppServices.Users
{
    public interface IUserStatusAppService
    {
        IList<UserStatusResult> Search(BaseFilter filter);
        Task<BasePageList<UserStatusResult>> GetAsync(UserStatusFilter filter);
        UserStatusResult Find(int id);
        UserStatusResult FindByName(string name);
        Task<int> CreateAsync(UserStatusModifyRequest request);
        Task<bool> UpdateAsync(UserStatusModifyRequest request);
    }
}
