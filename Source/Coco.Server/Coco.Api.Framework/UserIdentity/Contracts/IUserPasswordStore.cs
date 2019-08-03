using System.Threading;
using System.Threading.Tasks;

namespace Coco.Api.Framework.UserIdentity.Contracts
{
    public interface IUserPasswordStore<TUser> where TUser : class
    {
        Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the password hash for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose password hash to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, returning the password hash for the specified <paramref name="user"/>.</returns>
        Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken);
        string AddSaltToPassword(TUser user, string password);
    }
}
