using Camino.Service.Projections.Identity;
using Camino.Service.Projections.Filters;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Service.Projections.Request;
using Camino.Service.Projections.PageList;

namespace Camino.Service.Business.Users.Contracts
{
    public interface IUserBusiness
    {
        Task<UserProjection> CreateAsync(UserProjection userRequest);
        Task<UserProjection> FindByEmailAsync(string email);
        Task<UserProjection> FindByUsernameAsync(string username);
        Task DeleteAsync(long id);
        Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync(UserIdentifierUpdateRequest model);
        Task<UserProjection> FindByIdAsync(long id);
        Task<UserFullProjection> FindFullByIdAsync(long id);
        Task<UpdateItemRequest> UpdateInfoItemAsync(UpdateItemRequest model);
        Task<bool> ActiveAsync(long id);
        Task<UserProjection> UpdateAsync(UserProjection user);
        Task<BasePageList<UserFullProjection>> GetAsync(UserFilter filter);
        List<UserFullProjection> Search(string query = "", List<long> currentUserIds = null, int page = 1, int pageSize = 10);
    }
}
