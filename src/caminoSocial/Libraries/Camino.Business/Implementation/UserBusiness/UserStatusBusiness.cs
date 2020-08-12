using Camino.Business.Contracts;
using Camino.Business.Dtos.Identity;
using Camino.Data.Contracts;
using Camino.Data.Entities.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Camino.Business.Implementation.UserBusiness
{
    public class UserStatusBusiness : IUserStatusBusiness
    {
        private readonly IRepository<Status> _statusRepository;
        public UserStatusBusiness(IRepository<Status> statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public IList<StatusDto> Search(string query = "", int page = 1, int pageSize = 10)
        {
            if (query == null)
            {
                query = string.Empty;
            }

            query = query.ToLower();

            var data = _statusRepository.Get(x => string.IsNullOrEmpty(query) || x.Name.ToLower().Contains(query)
                || x.Description.ToLower().Contains(query));

            if (pageSize > 0)
            {
                data = data.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var users = data
                .Select(x => new StatusDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToList();

            return users;
        }
    }
}
