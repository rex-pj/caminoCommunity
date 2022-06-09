using Camino.Application.Contracts.AppServices.Authorization.Dtos;

namespace Camino.Application.Contracts.AppServices.Authorization
{
    public interface IRoleAppService
    {
        Task<long> CreateAsync(RoleModifyRequest request);
        Task<bool> DeleteAsync(long id);
        Task<RoleResult> FindAsync(long id);
        Task<RoleResult> FindByNameAsync(string name);
        Task<BasePageList<RoleResult>> GetAsync(RoleFilter filter);
        List<RoleResult> Search(BaseFilter filter, List<long> currentRoleIds = null);
        Task<bool> UpdateAsync(RoleModifyRequest request);
    }
}