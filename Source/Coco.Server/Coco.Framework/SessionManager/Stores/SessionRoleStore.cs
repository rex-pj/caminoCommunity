using AutoMapper;
using Coco.Business.Contracts;
using Coco.Framework.Models;
using Coco.Framework.SessionManager.Stores.Contracts;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Stores
{
    public class SessionRoleStore : ISessionRoleStore<ApplicationRole>, IDisposable
    {
        private readonly IRoleBusiness _roleBusiness;
        private readonly IMapper _mapper;
        public SessionRoleStore(IRoleBusiness roleBusiness, IMapper mapper)
        {
            _roleBusiness = roleBusiness;
            _mapper = mapper;
        }

        private bool _disposed;
        public virtual async Task<ApplicationRole> FindByNameAsync(string normalizedName)
        {
            ThrowIfDisposed();
            var role = await _roleBusiness.GetByNameAsync(normalizedName);

            return _mapper.Map<ApplicationRole>(role);
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationRole role)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var claims = new List<Claim>();
            // Todo: need implement claims
            return await Task.FromResult(claims);
        }

        public void Dispose()
        {
            _disposed = true;
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
