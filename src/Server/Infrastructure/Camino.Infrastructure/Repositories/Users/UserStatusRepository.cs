using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Identifiers;
using Camino.Core.Contracts.Data;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Requests.Identifiers;

namespace Camino.Infrastructure.Repositories.Users
{
    public class UserStatusRepository : IUserStatusRepository
    {
        private readonly IRepository<Status> _statusRepository;
        public UserStatusRepository(IRepository<Status> statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public IList<UserStatusResult> Search(BaseFilter filter)
        {
            var keyword = string.IsNullOrEmpty(filter.Keyword) ? filter.Keyword.ToLower() : "";
            var query = _statusRepository.Get(x => string.IsNullOrEmpty(keyword) || x.Name.ToLower().Contains(keyword)
                || x.Description.ToLower().Contains(keyword));

            if (filter.PageSize > 0)
            {
                query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            }

            var users = query
                .Select(x => new UserStatusResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToList();

            return users;
        }

        public async Task<BasePageList<UserStatusResult>> GetAsync(UserStatusFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var query = _statusRepository.Table
                .Select(x => new UserStatusResult()
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

            var result = new BasePageList<UserStatusResult>(statuses)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }

        public UserStatusResult Find(int id)
        {
            var status = _statusRepository.Get(x => x.Id == id)
                .Select(x => new UserStatusResult()
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name
                })
                .FirstOrDefault();

            return status;
        }

        public UserStatusResult FindByName(string name)
        {
            var status = _statusRepository.Get(x => x.Name == name)
                .Select(x => new UserStatusResult()
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name
                })
                .FirstOrDefault();

            return status;
        }

        public async Task<int> CreateAsync(UserStatusModifyRequest request)
        {
            var country = new Status()
            {
                Description = request.Description,
                Name = request.Name
            };

            var id = await _statusRepository.AddWithInt32EntityAsync(country);
            return id;
        }

        public async Task<bool> UpdateAsync(UserStatusModifyRequest request)
        {
            await _statusRepository.Get(x => x.Id == request.Id)
                .Set(x => x.Description, request.Description)
                .Set(x => x.Name, request.Name)
                .UpdateAsync();

            return true;
        }
    }
}
