using Camino.Application.Contracts.AppServices.Users;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Users;
using Camino.Core.Domains;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Camino.Core.Domains.Users.Repositories;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Camino.Application.AppServices.Users
{
    public class UserStatusAppService : IUserStatusAppService, IScopedDependency
    {
        private readonly IUserStatusRepository _statusRepository;
        private readonly IEntityRepository<Status> _statusEntityRepository;
        public UserStatusAppService(IUserStatusRepository statusRepository, IEntityRepository<Status> statusEntityRepository)
        {
            _statusRepository = statusRepository;
            _statusEntityRepository = statusEntityRepository;
        }

        public IList<UserStatusResult> Search(BaseFilter filter)
        {
            var keyword = string.IsNullOrEmpty(filter.Keyword) ? filter.Keyword.ToLower() : "";
            var query = _statusEntityRepository.Get(x => string.IsNullOrEmpty(keyword) || x.Name.ToLower().Contains(keyword)
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
            var query = _statusEntityRepository.Table
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
            var data = _statusRepository.Find(id);
            return new UserStatusResult
            {
                Id = data.Id,
                Description = data.Description,
                Name = data.Name
            };
        }

        public UserStatusResult FindByName(string name)
        {
            var data = _statusRepository.FindByName(name);
            return new UserStatusResult
            {
                Id = data.Id,
                Description = data.Description,
                Name = data.Name
            };
        }

        public async Task<int> CreateAsync(UserStatusModifyRequest request)
        {
            var status = new Status()
            {
                Description = request.Description,
                Name = request.Name
            };
            return await _statusRepository.CreateAsync(status);
        }

        public async Task<bool> UpdateAsync(UserStatusModifyRequest request)
        {
            var existing = _statusRepository.Find(request.Id);
            existing.Description = request.Description;
            existing.Name = request.Name;
            return await _statusRepository.UpdateAsync(existing);
        }
    }
}
