﻿using Camino.Service.Data.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Authorization.Contracts
{
    public interface IUserRoleBusiness
    {
        Task<IList<UserRoleProjection>> GetUserRolesAsync(long userId);
        Task<UserRoleProjection> FindUserRoleAsync(long userId, long roleId);
        Task<IList<UserProjection>> GetUsersInRoleAsync(long roleId);
        void Remove(UserRoleProjection userRoleRequest);
        void Add(UserRoleProjection userRoleRequest);
    }
}
