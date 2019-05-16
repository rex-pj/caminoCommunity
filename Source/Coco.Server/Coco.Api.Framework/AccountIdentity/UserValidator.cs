using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.AccountIdentity.Entities;
using Coco.Api.Framework.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity
{
    public class UserValidator : IUserValidator<ApplicationUser>
    {
        /// <summary>
        /// Creates a new instance of <see cref="UserValidator{TUser}"/>/
        /// </summary>
        /// <param name="errors">The <see cref="IdentityErrorDescriber"/> used to provider error messages.</param>
        public UserValidator()
        {

        }

        /// <summary>
        /// Validates the specified <paramref name="user"/> as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}"/> that can be used to retrieve user properties.</param>
        /// <param name="user">The user to validate.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the validation operation.</returns>
        //public virtual async Task<IdentityResult> ValidateAsync(IAccountManager<ApplicationUser> manager, ApplicationUser user)
        //{
        //    if (manager == null)
        //    {
        //        throw new ArgumentNullException(nameof(manager));
        //    }
        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }
        //    var errors = new List<IdentityError>();
        //    await ValidateUserName(manager, user, errors);
        //    if (manager.Options.User.RequireUniqueEmail)
        //    {
        //        await ValidateEmail(manager, user, errors);
        //    }
        //    return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : new IdentityResult(true);
        //}

        //private async Task ValidateUserName(IUserManager<ApplicationUser> manager, ApplicationUser user, ICollection<IdentityError> errors)
        //{
        //    //var userName = await manager.GetUserNameAsync(user);
        //    //if (string.IsNullOrWhiteSpace(userName))
        //    //{
        //    //    errors.Add(Describer.InvalidUserName(userName));
        //    //}
        //    //else if (!string.IsNullOrEmpty(manager.Options.User.AllowedUserNameCharacters) &&
        //    //    userName.Any(c => !manager.Options.User.AllowedUserNameCharacters.Contains(c)))
        //    //{
        //    //    errors.Add(Describer.InvalidUserName(userName));
        //    //}
        //    //else
        //    //{
        //    //    var owner = await manager.FindByNameAsync(userName);
        //    //    if (owner != null &&
        //    //        !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
        //    //    {
        //    //        errors.Add(Describer.DuplicateUserName(userName));
        //    //    }
        //    //}
        //}

        // make sure email is not empty, valid, and unique
        //private async Task ValidateEmail(IUserManager<ApplicationUser> manager, ApplicationUser user, List<IdentityError> errors)
        //{
        //    //var email = await manager.GetEmailAsync(user);
        //    //if (string.IsNullOrWhiteSpace(email))
        //    //{
        //    //    errors.Add(Describer.InvalidEmail(email));
        //    //    return;
        //    //}
        //    //if (!new EmailAddressAttribute().IsValid(email))
        //    //{
        //    //    errors.Add(Describer.InvalidEmail(email));
        //    //    return;
        //    //}
        //    //var owner = await manager.FindByEmailAsync(email);
        //    //if (owner != null &&
        //    //    !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
        //    //{
        //    //    errors.Add(Describer.DuplicateEmail(email));
        //    //}
        //}
    }
}
