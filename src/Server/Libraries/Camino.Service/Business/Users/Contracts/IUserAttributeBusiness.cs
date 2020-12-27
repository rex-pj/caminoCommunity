using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Users.Contracts
{
    public interface IUserAttributeBusiness
    {
        Task<UserAttribute> CreateOrUpdateAsync(long userId, string key, string value, DateTime? expiration = null);
        IEnumerable<UserAttribute> Create(IEnumerable<UserAttributeProjection> userAttributes);
        Task<UserAttribute> GetAsync(long userId, string key);
        IEnumerable<UserAttribute> Get(long userId);
        Task<IEnumerable<UserAttribute>> GetAsync(long userId);
        Task<IEnumerable<UserAttribute>> CreateOrUpdateAsync(IEnumerable<UserAttributeProjection> userAttributes);
        Task<bool> DeleteAsync(long userId, string key, string value);
        Task<bool> DeleteAsync(long userId, string key);
    }
}
