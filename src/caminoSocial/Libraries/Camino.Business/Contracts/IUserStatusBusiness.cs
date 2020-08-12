using Camino.Business.Dtos.Identity;
using System.Collections.Generic;

namespace Camino.Business.Contracts
{
    public interface IUserStatusBusiness
    {
        IList<StatusDto> Search(string query = "", int page = 1, int pageSize = 10);
    }
}
