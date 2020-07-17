using AutoMapper;
using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Core.Dtos.Identity;
using Coco.Core.Entities.Identity;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation
{
    public class RoleBusiness : IRoleBusiness
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public RoleBusiness(IMapper mapper, IRepository<Role> roleRepository, IRepository<User> userRepository)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        #region CRUD
        public async Task<long> AddAsync(RoleDto roleModel)
        {
            if (roleModel == null)
            {
                throw new ArgumentNullException(nameof(roleModel));
            }

            var role = _mapper.Map<Role>(roleModel);
            role.UpdatedDate = DateTime.UtcNow;
            role.CreatedDate = DateTime.UtcNow;

            role.Id = await _roleRepository.AddWithInt64EntityAsync(role);
            return role.Id;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var role = _roleRepository.FirstOrDefault(x => x.Id == id);
            await _roleRepository.DeleteAsync(role);

            return true;
        }

        public async Task<RoleDto> FindAsync(long id)
        {
            var existRole = await (from role in _roleRepository.Table
                                   join createdBy in _userRepository.Table
                                   on role.CreatedById equals createdBy.Id
                                   join updatedBy in _userRepository.Table
                                   on role.UpdatedById equals updatedBy.Id
                                   where role.Id == id
                                   select new RoleDto()
                                   {
                                       CreatedByName = createdBy.Lastname + " " + createdBy.Firstname,
                                       UpdatedByName = updatedBy.Lastname + " " + updatedBy.Firstname,
                                       CreatedById = role.CreatedById,
                                       CreatedDate = role.CreatedDate,
                                       Description = role.Description,
                                       Id = role.Id,
                                       Name = role.Name,
                                       UpdatedById = role.UpdatedById,
                                       UpdatedDate = role.UpdatedDate,
                                       ConcurrencyStamp = role.ConcurrencyStamp
                                   }).FirstOrDefaultAsync();

            if (existRole == null)
            {
                return null;
            }

            return existRole;
        }

        public List<RoleDto> Search(string query = "", int page = 1, int pageSize = 10)
        {
            if (query == null)
            {
                query = string.Empty;
            }

            query = query.ToLower();

            var data = _roleRepository.Get(x => string.IsNullOrEmpty(query) || x.Name.ToLower().Contains(query));

            data = data.Skip(page).Take(pageSize);
            if (pageSize > 0)
            {
                data = data.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var users = data
                .Select(x => new RoleDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToList();

            return users;
        }

        public RoleDto FindByName(string name)
        {
            var exist = _roleRepository.Get(x => x.Name == name).FirstOrDefault();
            if (exist == null)
            {
                return null;
            }

            return _mapper.Map<RoleDto>(exist);
        }

        public async Task<RoleDto> FindByNameAsync(string name)
        {
            name = name.ToLower();
            var exist = await _roleRepository.Get(x => x.Name.ToLower() == name)
                .FirstOrDefaultAsync();
            if (exist == null)
            {
                return null;
            }

            return _mapper.Map<RoleDto>(exist);
        }

        public async Task<List<RoleDto>> GetAsync()
        {
            var roles = await (from role in _roleRepository.Table
                         join createdBy in _userRepository.Table
                         on role.CreatedById equals createdBy.Id
                         join updatedBy in _userRepository.Table
                         on role.UpdatedById equals updatedBy.Id select new RoleDto
                         {
                             Id = role.Id,
                             Name = role.Name,
                             CreatedById = role.CreatedById,
                             CreatedByName = createdBy.Lastname + " " + createdBy.Firstname,
                             CreatedDate = role.CreatedDate,
                             Description = role.Description,
                             UpdatedById = role.UpdatedById,
                             UpdatedByName = updatedBy.Lastname + " " + updatedBy.Firstname,
                             UpdatedDate = role.UpdatedDate
                         }).ToListAsync();

            return roles;
        }

        public async Task<bool> UpdateAsync(RoleDto roleModel)
        {
            if (roleModel.Id <= 0)
            {
                throw new ArgumentException("Role Id");
            }

            var exist = _roleRepository.FirstOrDefault(x => x.Id == roleModel.Id);
            exist.Description = roleModel.Description;
            exist.Name = roleModel.Name;
            exist.UpdatedById = roleModel.UpdatedById;
            exist.UpdatedDate = DateTime.UtcNow;
            exist.ConcurrencyStamp = roleModel.ConcurrencyStamp;

            await _roleRepository.UpdateAsync(exist);
            return true;
        }

        public async Task<RoleDto> GetByNameAsync(string name)
        {
            var role = (await _roleRepository.GetAsync(x => x.Name.Equals(name))).FirstOrDefault();

            var roleModel = _mapper.Map<RoleDto>(role);
            return roleModel;
        }
        #endregion
    }
}
