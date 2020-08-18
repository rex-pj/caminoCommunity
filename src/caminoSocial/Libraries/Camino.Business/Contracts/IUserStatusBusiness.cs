using Camino.Business.Dtos.General;
using Camino.Business.Dtos.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Business.Contracts
{
    public interface IUserStatusBusiness
    {
        IList<UserStatusDto> Search(string query = "", int page = 1, int pageSize = 10);
        Task<PageListDto<UserStatusDto>> GetAsync(UserStatusFilterDto filter);
        UserStatusDto Find(int id);
        UserStatusDto FindByName(string name);
        int Add(UserStatusDto statusDto);
        UserStatusDto Update(UserStatusDto statusDto);
    }
}
