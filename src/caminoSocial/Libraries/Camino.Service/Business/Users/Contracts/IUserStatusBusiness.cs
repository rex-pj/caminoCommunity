using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Identity;
using Camino.Service.Projections.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Users.Contracts
{
    public interface IUserStatusBusiness
    {
        IList<UserStatusProjection> Search(string query = "", int page = 1, int pageSize = 10);
        Task<BasePageList<UserStatusProjection>> GetAsync(UserStatusFilter filter);
        UserStatusProjection Find(int id);
        UserStatusProjection FindByName(string name);
        int Add(UserStatusProjection statusRequest);
        UserStatusProjection Update(UserStatusProjection statusRequest);
    }
}
