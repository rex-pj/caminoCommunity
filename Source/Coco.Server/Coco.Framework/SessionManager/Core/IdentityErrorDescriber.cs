using Coco.Framework.Commons.Constants;
using Coco.Framework.Commons.Helpers;
using Coco.Common.Const;
using Coco.Commons.Models;

namespace Coco.Framework.SessionManager.Core
{
    /// <summary>
    /// Service to enable localization for application facing identity errors.
    /// </summary>
    /// <remarks>
    /// These errors are returned to controllers and are generally used as display messages to end users.
    /// </remarks>
    public class IdentityErrorDescriber
    {
        /// <summary>
        /// Returns the default <see cref="CommonError"/>.
        /// </summary>
        /// <returns>The default <see cref="CommonError"/>.</returns>
        public virtual CommonError DefaultError()
        {
            return new CommonError
            {
                Code = nameof(DefaultError),
                Message = ExceptionConstant.DefaultError
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating a concurrency failure.
        /// </summary>
        /// <returns>An <see cref="CommonError"/> indicating a concurrency failure.</returns>
        public virtual CommonError ConcurrencyFailure()
        {
            return new CommonError
            {
                Code = nameof(ConcurrencyFailure),
                Message = ExceptionConstant.ConcurrencyFailure
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating a password mismatch.
        /// </summary>
        /// <returns>An <see cref="CommonError"/> indicating a password mismatch.</returns>
        public virtual CommonError PasswordMismatch()
        {
            return new CommonError
            {
                Code = nameof(PasswordMismatch),
                Message = ExceptionConstant.PasswordMismatch
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> an unexpected error has occurred.
        /// </summary>
        /// <returns>An <see cref="CommonError"/> an unexpected error has occurred.</returns>
        public virtual CommonError UnexpectedErrorOccurred()
        {
            return new CommonError
            {
                Code = nameof(UnexpectedErrorOccurred),
                Message = ExceptionConstant.UnexpectedErrorOccurred
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating an invalid token.
        /// </summary>
        /// <returns>An <see cref="CommonError"/> indicating an invalid token.</returns>
        public virtual CommonError InvalidToken()
        {
            return new CommonError
            {
                Code = nameof(InvalidToken),
                Message = ExceptionConstant.InvalidToken
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating a recovery code was not redeemed.
        /// </summary>
        /// <returns>An <see cref="CommonError"/> indicating a recovery code was not redeemed.</returns>
        public virtual CommonError RecoveryCodeRedemptionFailed()
        {
            return new CommonError
            {
                Code = nameof(RecoveryCodeRedemptionFailed),
                Message = ExceptionConstant.RecoveryCodeRedemptionFailed
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating an external login is already associated with an user.
        /// </summary>
        /// <returns>An <see cref="CommonError"/> indicating an external login is already associated with an user.</returns>
        public virtual CommonError LoginAlreadyAssociated()
        {
            return new CommonError
            {
                Code = nameof(LoginAlreadyAssociated),
                Message = ExceptionConstant.LoginAlreadyAssociated
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating the specified user <paramref name="userName"/> is invalid.
        /// </summary>
        /// <param name="userName">The user name that is invalid.</param>
        /// <returns>An <see cref="CommonError"/> indicating the specified user <paramref name="userName"/> is invalid.</returns>
        public virtual CommonError InvalidUserName(string userName)
        {
            return new CommonError
            {
                Code = nameof(InvalidUserName),
                Message = MessageResources.FormatInvalidUserName(userName)
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating the specified <paramref name="email"/> is invalid.
        /// </summary>
        /// <param name="email">The email that is invalid.</param>
        /// <returns>An <see cref="CommonError"/> indicating the specified <paramref name="email"/> is invalid.</returns>
        public virtual CommonError InvalidEmail(string email)
        {
            return new CommonError
            {
                Code = nameof(InvalidEmail),
                Message = MessageResources.FormatInvalidEmail(email)
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating the specified <paramref name="userName"/> already exists.
        /// </summary>
        /// <param name="userName">The user name that already exists.</param>
        /// <returns>An <see cref="CommonError"/> indicating the specified <paramref name="userName"/> already exists.</returns>
        public virtual CommonError DuplicateUserName(string userName)
        {
            return new CommonError
            {
                Code = nameof(DuplicateUserName),
                Message = MessageResources.FormatDuplicateUserName(userName)
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating the specified <paramref name="email"/> is already associated with an user.
        /// </summary>
        /// <param name="email">The email that is already associated with an user.</param>
        /// <returns>An <see cref="CommonError"/> indicating the specified <paramref name="email"/> is already associated with an user.</returns>
        public virtual CommonError DuplicateEmail(string email)
        {
            return new CommonError
            {
                Code = nameof(DuplicateEmail),
                Message = MessageResources.FormatDuplicateEmail(email)
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating the specified <paramref name="role"/> name is invalid.
        /// </summary>
        /// <param name="role">The invalid role.</param>
        /// <returns>An <see cref="CommonError"/> indicating the specific role <paramref name="role"/> name is invalid.</returns>
        public virtual CommonError InvalidRoleName(string role)
        {
            return new CommonError
            {
                Code = nameof(InvalidRoleName),
                Message = MessageResources.FormatInvalidRoleName(role)
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating the specified <paramref name="role"/> name already exists.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="CommonError"/> indicating the specific role <paramref name="role"/> name already exists.</returns>
        public virtual CommonError DuplicateRoleName(string role)
        {
            return new CommonError
            {
                Code = nameof(DuplicateRoleName),
                Message = MessageResources.FormatDuplicateRoleName(role)
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating a user already has a password.
        /// </summary>
        /// <returns>An <see cref="CommonError"/> indicating a user already has a password.</returns>
        public virtual CommonError UserAlreadyHasPassword()
        {
            return new CommonError
            {
                Code = nameof(UserAlreadyHasPassword),
                Message = ExceptionConstant.UserAlreadyHasPassword
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating a user already actived.
        /// </summary>
        /// <returns>An <see cref="CommonError"/> indicating a user already actived.</returns>
        public virtual CommonError UserAlreadyActived()
        {
            return new CommonError
            {
                Code = nameof(UserAlreadyActived),
                Message = ExceptionConstant.UserAlreadyActived
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating user lockout is not enabled.
        /// </summary>
        /// <returns>An <see cref="CommonError"/> indicating user lockout is not enabled.</returns>
        public virtual CommonError UserLockoutNotEnabled()
        {
            return new CommonError
            {
                Code = nameof(UserLockoutNotEnabled),
                Message = ExceptionConstant.UserLockoutNotEnabled
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating a user is already in the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="CommonError"/> indicating a user is already in the specified <paramref name="role"/>.</returns>
        public virtual CommonError UserAlreadyInRole(string role)
        {
            return new CommonError
            {
                Code = nameof(UserAlreadyInRole),
                Message = MessageResources.FormatUserAlreadyInRole(role)
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating a user is not in the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="CommonError"/> indicating a user is not in the specified <paramref name="role"/>.</returns>
        public virtual CommonError UserNotInRole(string role)
        {
            return new CommonError
            {
                Code = nameof(UserNotInRole),
                Message = MessageResources.FormatUserNotInRole(role)
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating a password of the specified <paramref name="length"/> does not meet the minimum length requirements.
        /// </summary>
        /// <param name="length">The length that is not long enough.</param>
        /// <returns>An <see cref="CommonError"/> indicating a password of the specified <paramref name="length"/> does not meet the minimum length requirements.</returns>
        public virtual CommonError PasswordTooShort(int length)
        {
            return new CommonError
            {
                Code = nameof(PasswordTooShort),
                Message = MessageResources.FormatPasswordTooShort(length)
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating a password of the specified <paramref name="length"/> does not meet the minimum length requirements.
        /// </summary>
        /// <param name="length">The length that is too long.</param>
        /// <returns>An <see cref="CommonError"/> indicating a password of the specified <paramref name="length"/> does not meet the maximum length requirements.</returns>
        public virtual CommonError PasswordTooLong(int length)
        {
            return new CommonError
            {
                Code = nameof(PasswordTooLong),
                Message = MessageResources.FormatPasswordTooLong(length)
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating a password does not meet the minimum number <paramref name="uniqueChars"/> of unique chars.
        /// </summary>
        /// <param name="uniqueChars">The number of different chars that must be used.</param>
        /// <returns>An <see cref="CommonError"/> indicating a password does not meet the minimum number <paramref name="uniqueChars"/> of unique chars.</returns>
        public virtual CommonError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new CommonError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Message = MessageResources.FormatPasswordRequiresUniqueChars(uniqueChars)
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating a password entered does not contain a non-alphanumeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="CommonError"/> indicating a password entered does not contain a non-alphanumeric character.</returns>
        public virtual CommonError PasswordRequiresNonAlphanumeric()
        {
            return new CommonError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Message = ExceptionConstant.PasswordRequiresNonAlphanumeric
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating a password entered does not contain a numeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="CommonError"/> indicating a password entered does not contain a numeric character.</returns>
        public virtual CommonError PasswordRequiresDigit()
        {
            return new CommonError
            {
                Code = nameof(PasswordRequiresDigit),
                Message = ExceptionConstant.PasswordRequiresDigit
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating a password entered does not contain a lower case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="CommonError"/> indicating a password entered does not contain a lower case letter.</returns>
        public virtual CommonError PasswordRequiresLower()
        {
            return new CommonError
            {
                Code = nameof(PasswordRequiresLower),
                Message = ExceptionConstant.PasswordRequiresLower
            };
        }

        /// <summary>
        /// Returns an <see cref="CommonError"/> indicating a password entered does not contain an upper case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="CommonError"/> indicating a password entered does not contain an upper case letter.</returns>
        public virtual CommonError PasswordRequiresUpper()
        {
            return new CommonError
            {
                Code = nameof(PasswordRequiresUpper),
                Message = ExceptionConstant.PasswordRequiresUpper
            };
        }

        public virtual CommonError PhotoSizeInvalid()
        {
            return new CommonError
            {
                Code = nameof(PhotoSizeInvalid),
                Message = ErrorMessageConst.PhotoSizeInvalidException
            };
        }
    }
}
