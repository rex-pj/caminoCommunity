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

        public IList<UserStatusDto> Search(string query = "", int page = 1, int pageSize = 10)
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
                .Select(x => new UserStatusDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToList();

            return users;
        }

        public List<UserStatusDto> GetAll()
        {
            return _statusRepository.Get()
                .Select(x => new UserStatusDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToList();
        }

        public UserStatusDto Find(int id)
        {
            var status = _statusRepository.Get(x => x.Id == id)
                .Select(x => new UserStatusDto()
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name
                })
                .FirstOrDefault();

            return status;
        }

        public UserStatusDto FindByName(string name)
        {
            var status = _statusRepository.Get(x => x.Name == name)
                .Select(x => new UserStatusDto()
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name
                })
                .FirstOrDefault();

            return status;
        }

        public int Add(UserStatusDto statusDto)
        {
            var country = new Status()
            {
                Description = statusDto.Description,
                Name = statusDto.Name
            };

            var id = _statusRepository.AddWithInt32Entity(country);
            return id;
        }

        public UserStatusDto Update(UserStatusDto statusDto)
        {
            var exist = _statusRepository.FirstOrDefault(x => x.Id == statusDto.Id);
            if (exist == null)
            {
                return null;
            }
            exist.Description = statusDto.Description;
            exist.Name = statusDto.Name;

            _statusRepository.Update(exist);
            return statusDto;
        }
    }
}
