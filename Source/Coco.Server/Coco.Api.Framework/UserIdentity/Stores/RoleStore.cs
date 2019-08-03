using Coco.Api.Framework.UserIdentity.Contracts;

namespace Coco.Api.Framework.UserIdentity.Stores
{
    public class RoleStore :
        IRoleStore
    {
        //        private readonly IRoleBusiness _roleBusiness;

        //        public ApplicationRoleStore(IRoleBusiness roleBusiness)
        //        {
        //            _roleBusiness = roleBusiness;
        //        }

        //        #region IRoleStore<IdentityRole> Members
        //        public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        //        {
        //            try
        //            {
        //                if (cancellationToken != null)
        //                {
        //                    cancellationToken.ThrowIfCancellationRequested();
        //                }

        //                if (role == null)
        //                {
        //                    throw new ArgumentNullException(nameof(role));
        //                }

        //                RoleModel roleModel = GetRoleModel(role);

        //                _roleBusiness.Add(roleModel);

        //                return Task.FromResult(IdentityResult.Success);
        //            }
        //            catch (Exception ex)
        //            {
        //                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
        //            }
        //        }

        //        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        //        {
        //            try
        //            {
        //                if (cancellationToken != null)
        //                {
        //                    cancellationToken.ThrowIfCancellationRequested();
        //                }

        //                if (role == null)
        //                {
        //                    throw new ArgumentNullException(nameof(role));
        //                }

        //                _roleBusiness.Delete(role.Id);

        //                return Task.FromResult(IdentityResult.Success);
        //            }
        //            catch (Exception ex)
        //            {
        //                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
        //            }
        //        }

        //        public Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        //        {
        //            if (cancellationToken != null)
        //            {
        //                cancellationToken.ThrowIfCancellationRequested();
        //            }

        //            if (string.IsNullOrWhiteSpace(roleId))
        //            {
        //                throw new ArgumentNullException(nameof(roleId));
        //            }

        //            byte id = byte.Parse(roleId);

        //            var roleModel = _roleBusiness.Find(id);
        //            return Task.FromResult(GetIdentityRole(roleModel));
        //        }

        //        public Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        //        {
        //            if (cancellationToken != null)
        //                cancellationToken.ThrowIfCancellationRequested();

        //            if (string.IsNullOrWhiteSpace(normalizedRoleName))
        //                throw new ArgumentNullException(nameof(normalizedRoleName));

        //            var roleModel = _roleBusiness.GetByName(normalizedRoleName);
        //            return Task.FromResult(GetIdentityRole(roleModel));
        //        }

        //        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        //        {
        //            if (cancellationToken != null)
        //            {
        //                cancellationToken.ThrowIfCancellationRequested();
        //            }

        //            if (role == null)
        //            {
        //                throw new ArgumentNullException(nameof(role));
        //            }

        //            return Task.FromResult(role.NormalizedName);
        //        }

        //        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        //        {
        //            if (cancellationToken != null)
        //            {
        //                cancellationToken.ThrowIfCancellationRequested();
        //            }

        //            if (role == null)
        //            {
        //                throw new ArgumentNullException(nameof(role));
        //            }

        //            return Task.FromResult(role.Id.ToString());
        //        }

        //        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        //        {
        //            if (cancellationToken != null)
        //            {
        //                cancellationToken.ThrowIfCancellationRequested();
        //            }

        //            if (role == null)
        //            {
        //                throw new ArgumentNullException(nameof(role));
        //            }                

        //            return Task.FromResult(role.Name);
        //        }

        //        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        //        {
        //            if (cancellationToken != null)
        //            {
        //                cancellationToken.ThrowIfCancellationRequested();
        //            }                

        //            if (role == null)
        //            {
        //                throw new ArgumentNullException(nameof(role));
        //            }

        //            role.NormalizedName = normalizedName;

        //            return Task.CompletedTask;
        //        }

        //        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        //        {
        //            if (cancellationToken != null)
        //            {
        //                cancellationToken.ThrowIfCancellationRequested();
        //            }

        //            if (role == null)
        //            {
        //                throw new ArgumentNullException(nameof(role));
        //            }

        //            role.Name = roleName;

        //            return Task.CompletedTask;
        //        }

        //        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        //        {
        //            try
        //            {
        //                if (cancellationToken != null)
        //                {
        //                    cancellationToken.ThrowIfCancellationRequested();
        //                }


        //                if (role == null)
        //                {
        //                    throw new ArgumentNullException(nameof(role));
        //                }

        //                var roleModel = GetRoleModel(role);

        //                _roleBusiness.Update(roleModel);

        //                return Task.FromResult(IdentityResult.Success);
        //            }
        //            catch (Exception ex)
        //            {
        //                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
        //            }
        //        }

        //        public void Dispose()
        //        {
        //            // Lifetimes of dependencies are managed by the IoC container, so disposal here is unnecessary.
        //        }
        //        #endregion

        //        #region Private Methods
        //        private RoleModel GetRoleModel(ApplicationRole value)
        //        {
        //            return value == null
        //                ? default(RoleModel)
        //                : new RoleModel
        //                {
        //                    Id = value.Id,
        //                    Name = value.Name
        //                };
        //        }

        //        private ApplicationRole GetIdentityRole(RoleModel value)
        //        {
        //            return value == null
        //                ? default(ApplicationRole)
        //                : new ApplicationRole
        //                {
        //                    Id = value.Id,
        //                    Name = value.Name
        //                };
        //        }
        //        #endregion
    }
}
