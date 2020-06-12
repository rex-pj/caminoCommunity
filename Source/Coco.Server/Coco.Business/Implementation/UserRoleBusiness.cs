﻿using AutoMapper;
using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.Auth;
using Coco.Entities.Dtos.User;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation
{
    public class UserRoleBusiness : IUserRoleBusiness
    {
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IMapper _mapper;

        public UserRoleBusiness(IRepository<UserRole> userRoleRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<IList<UserRoleDto>> GetUserRolesAsync(long userId)
        {
            var userRoles = (await _userRoleRepository.GetAsync(x => x.UserId == userId))
                .Select(x => new UserRoleDto()
                {
                    RoleId = x.RoleId,
                    RoleName = x.Role.Name,
                    UserId = x.UserId
                }).ToList();

            return userRoles;
        }

        public async Task<UserRoleDto> FindUserRoleAsync(long userId, long roleId)
        {
            var userRole = (await _userRoleRepository.GetAsync(x => x.UserId == userId && x.RoleId == roleId))
                .FirstOrDefault();

            return _mapper.Map<UserRoleDto>(userRole);
        }

        public async Task<IList<UserDto>> GetUsersInRoleAsync(long roleId)
        {
            var existUserRoles = await _userRoleRepository.Get(x => x.RoleId == roleId)
                .Select(x => new UserDto()
                {
                    Id = x.UserId,
                    DisplayName = x.User.DisplayName,
                    Lastname = x.User.Lastname,
                    Firstname = x.User.Firstname,
                    UserName = x.User.UserName,
                    Email = x.User.Email,
                    IsActived = x.User.IsActived,
                    IsEmailConfirmed = x.User.IsEmailConfirmed,
                    StatusId = x.User.StatusId
                }).ToListAsync();

            return existUserRoles;
        }

        public void Add(UserRoleDto userRoleDto)
        {
            var userRole = _mapper.Map<UserRole>(userRoleDto);
            _userRoleRepository.Add(userRole);
        }

        public void Remove(UserRoleDto userRoleDto)
        {
            var userRole = _mapper.Map<UserRole>(userRoleDto);
            _userRoleRepository.Delete(userRole);
        }
    }
}
