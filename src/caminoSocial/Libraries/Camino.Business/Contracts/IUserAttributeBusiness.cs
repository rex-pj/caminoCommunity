using Camino.Business.Dtos.Identity;
using Camino.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Business.Contracts
{
    public interface IUserAttributeBusiness
    {
        Task<UserAttribute> CreateOrUpdateAsync(long userId, string key, string value, DateTime? expiration = null);
        IEnumerable<UserAttribute> Create(IEnumerable<UserAttributeDto> userAttributes);
        Task<UserAttribute> GetAsync(long userId, string key);
        IEnumerable<UserAttribute> Get(long userId);
        Task<IEnumerable<UserAttribute>> GetAsync(long userId);
        Task<IEnumerable<UserAttribute>> CreateOrUpdateAsync(IEnumerable<UserAttributeDto> userAttributes);
        Task<bool> DeleteAsync(long userId, string key, string value);
        Task<bool> DeleteAsync(long userId, string key);
    }
}
