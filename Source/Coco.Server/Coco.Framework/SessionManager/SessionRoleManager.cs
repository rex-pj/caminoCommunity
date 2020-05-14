using Coco.Framework.Models;
using Coco.Framework.SessionManager.Contracts;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager
{
    public class SessionRoleManager : ISessionRoleManager<ApplicationRole>
    {
        private bool _disposed;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly ISessionRoleStore<ApplicationRole> _sessionRoleStore;
        public SessionRoleManager(ILookupNormalizer lookupNormalizer, ISessionRoleStore<ApplicationRole> sessionRoleStore)
        {
            _sessionRoleStore = sessionRoleStore;
            _lookupNormalizer = lookupNormalizer;
        }

        public virtual async Task<ApplicationRole> FindByNameAsync(string roleName)
        {
            ThrowIfDisposed();
            if (roleName == null)
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            return await _sessionRoleStore.FindByNameAsync(NormalizeKey(roleName));
        }

        public virtual Task<IList<Claim>> GetClaimsAsync(ApplicationRole role)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return _sessionRoleStore.GetClaimsAsync(role);
        }

        public virtual string NormalizeKey(string key)
        {
            return (_lookupNormalizer == null) ? key : _lookupNormalizer.NormalizeName(key);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _sessionRoleStore.Dispose();
            }
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
