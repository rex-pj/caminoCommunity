﻿using AutoMapper;
using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.Auth;
using Coco.IdentityDAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation
{
    public class RoleBusiness : IRoleBusiness
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public RoleBusiness(IMapper mapper, IdentityDbContext dbContext, IRepository<Role> roleRepository, IRepository<User> userRepository)
        {
            _dbContext = dbContext;
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

            _roleRepository.Add(role);
            await _dbContext.SaveChangesAsync();
            return role.Id;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var role = _roleRepository.Find(id);
            _roleRepository.Delete(role);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<RoleDto> FindAsync(long id)
        {
            var exist = await _roleRepository.FindAsync(id);
            if (exist == null)
            {
                return null;
            }

            var createdByUser = _userRepository.Find(exist.CreatedById);
            var updatedByUser = _userRepository.Find(exist.UpdatedById);

            var role = _mapper.Map<RoleDto>(exist);
            role.CreatedByName = createdByUser.DisplayName;
            role.UpdatedByName = updatedByUser.DisplayName;
            
            return role;
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
            var roles = (await _roleRepository.GetAsync())
                .Select(a => new RoleDto {
                    Id = a.Id,
                    Name = a.Name,
                    CreatedById = a.CreatedById,
                    CreatedDate = a.CreatedDate,
                    Description = a.Description,
                    UpdatedById = a.UpdatedById,
                    UpdatedDate = a.UpdatedDate
                });

            var createdByIds = roles.Select(x => x.CreatedById).ToArray();
            var updatedByIds = roles.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).ToList();
            var updatedByUsers = _userRepository.Get(x => updatedByIds.Contains(x.Id)).ToList();

            foreach (var role in roles)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == role.CreatedById);
                role.CreatedByName = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == role.CreatedById);
                role.UpdatedByName = updatedBy.DisplayName;
            }

            var roleDtos = _mapper.Map<List<RoleDto>>(roles);
            return roleDtos;
        }

        public async Task<bool> UpdateAsync(RoleDto roleModel)
        {
            if (roleModel.Id <= 0)
            {
                throw new ArgumentException("Role Id");
            }

            var exist = _roleRepository.Find(roleModel.Id);
            exist.Description = roleModel.Description;
            exist.Name = roleModel.Name;
            exist.UpdatedById = roleModel.UpdatedById;
            exist.UpdatedDate = DateTime.UtcNow;
            exist.ConcurrencyStamp = roleModel.ConcurrencyStamp;

            _roleRepository.Update(exist);
            await _dbContext.SaveChangesAsync();

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
