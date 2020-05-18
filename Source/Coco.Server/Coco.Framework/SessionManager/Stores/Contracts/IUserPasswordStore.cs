//using Coco.Framework.Models;
//using System.Threading.Tasks;

//namespace Coco.Framework.SessionManager.Stores.Contracts
//{
//    public interface IUserPasswordStore<TUser> where TUser : class
//    {
//        Task SetPasswordHashAsync(TUser user, string passwordHash);
//        /// <summary>
//        /// Gets the password hash for the specified <paramref name="user"/>.
//        /// </summary>
//        /// <param name="user">The user whose password hash to retrieve.</param>
//        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, returning the password hash for the specified <paramref name="user"/>.</returns>
//        Task<string> GetPasswordHashAsync(TUser user);
//        string AddSaltToPassword(TUser user, string password);
//        Task<UserTokenResult> ChangePasswordAsync(long userId, string currentPassword, string newPassword);
//    }
//}
