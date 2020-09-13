using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Identity;
using Camino.Data.Contracts;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Users.Contracts;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.PageList;

namespace Camino.Service.Business.Users
{
    public class UserStatusBusiness : IUserStatusBusiness
    {
        private readonly IRepository<Status> _statusRepository;
        public UserStatusBusiness(IRepository<Status> statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public IList<UserStatusProjection> Search(string query = "", int page = 1, int pageSize = 10)
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
                .Select(x => new UserStatusProjection()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToList();

            return users;
        }

        public async Task<BasePageList<UserStatusProjection>> GetAsync(UserStatusFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var query = _statusRepository.Table
                .Select(x => new UserStatusProjection()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                });

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search) || x.Description.ToLower().Contains(search));
            }

            var filteredNumber = query.Select(x => x.Id).Count();

            var statuses = await query.OrderBy(x => x.Id)
                .Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize)
                                         .ToListAsync();

            var result = new BasePageList<UserStatusProjection>(statuses)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }

        public UserStatusProjection Find(int id)
        {
            var status = _statusRepository.Get(x => x.Id == id)
                .Select(x => new UserStatusProjection()
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name
                })
                .FirstOrDefault();

            return status;
        }

        public UserStatusProjection FindByName(string name)
        {
            var status = _statusRepository.Get(x => x.Name == name)
                .Select(x => new UserStatusProjection()
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name
                })
                .FirstOrDefault();

            return status;
        }

        public int Add(UserStatusProjection statusRequest)
        {
            var country = new Status()
            {
                Description = statusRequest.Description,
                Name = statusRequest.Name
            };

            var id = _statusRepository.AddWithInt32Entity(country);
            return id;
        }

        public UserStatusProjection Update(UserStatusProjection statusRequest)
        {
            var exist = _statusRepository.FirstOrDefault(x => x.Id == statusRequest.Id);
            if (exist == null)
            {
                return null;
            }
            exist.Description = statusRequest.Description;
            exist.Name = statusRequest.Name;

            _statusRepository.Update(exist);
            return statusRequest;
        }
    }
}
