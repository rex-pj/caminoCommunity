using Coco.Api.Framework.Commons.Constants;
using Coco.Api.Framework.Commons.Helpers;
using Coco.Api.Framework.Models;
using Coco.Common.Const;

namespace Coco.Api.Framework.UserIdentity.Entities
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
        /// Returns the default <see cref="ApiError"/>.
        /// </summary>
        /// <returns>The default <see cref="ApiError"/>.</returns>
        public virtual ApiError DefaultError()
        {
            return new ApiError
            {
                Code = nameof(DefaultError),
                Description = ExceptionConstant.DefaultError
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating a concurrency failure.
        /// </summary>
        /// <returns>An <see cref="ApiError"/> indicating a concurrency failure.</returns>
        public virtual ApiError ConcurrencyFailure()
        {
            return new ApiError
            {
                Code = nameof(ConcurrencyFailure),
                Description = ExceptionConstant.ConcurrencyFailure
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating a password mismatch.
        /// </summary>
        /// <returns>An <see cref="ApiError"/> indicating a password mismatch.</returns>
        public virtual ApiError PasswordMismatch()
        {
            return new ApiError
            {
                Code = nameof(PasswordMismatch),
                Description = ExceptionConstant.PasswordMismatch
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating an invalid token.
        /// </summary>
        /// <returns>An <see cref="ApiError"/> indicating an invalid token.</returns>
        public virtual ApiError InvalidToken()
        {
            return new ApiError
            {
                Code = nameof(InvalidToken),
                Description = ExceptionConstant.InvalidToken
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating a recovery code was not redeemed.
        /// </summary>
        /// <returns>An <see cref="ApiError"/> indicating a recovery code was not redeemed.</returns>
        public virtual ApiError RecoveryCodeRedemptionFailed()
        {
            return new ApiError
            {
                Code = nameof(RecoveryCodeRedemptionFailed),
                Description = ExceptionConstant.RecoveryCodeRedemptionFailed
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating an external login is already associated with an account.
        /// </summary>
        /// <returns>An <see cref="ApiError"/> indicating an external login is already associated with an account.</returns>
        public virtual ApiError LoginAlreadyAssociated()
        {
            return new ApiError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = ExceptionConstant.LoginAlreadyAssociated
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating the specified user <paramref name="userName"/> is invalid.
        /// </summary>
        /// <param name="userName">The user name that is invalid.</param>
        /// <returns>An <see cref="ApiError"/> indicating the specified user <paramref name="userName"/> is invalid.</returns>
        public virtual ApiError InvalidUserName(string userName)
        {
            return new ApiError
            {
                Code = nameof(InvalidUserName),
                Description = MessageResources.FormatInvalidUserName(userName)
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating the specified <paramref name="email"/> is invalid.
        /// </summary>
        /// <param name="email">The email that is invalid.</param>
        /// <returns>An <see cref="ApiError"/> indicating the specified <paramref name="email"/> is invalid.</returns>
        public virtual ApiError InvalidEmail(string email)
        {
            return new ApiError
            {
                Code = nameof(InvalidEmail),
                Description = MessageResources.FormatInvalidEmail(email)
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating the specified <paramref name="userName"/> already exists.
        /// </summary>
        /// <param name="userName">The user name that already exists.</param>
        /// <returns>An <see cref="ApiError"/> indicating the specified <paramref name="userName"/> already exists.</returns>
        public virtual ApiError DuplicateUserName(string userName)
        {
            return new ApiError
            {
                Code = nameof(DuplicateUserName),
                Description = MessageResources.FormatDuplicateUserName(userName)
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating the specified <paramref name="email"/> is already associated with an account.
        /// </summary>
        /// <param name="email">The email that is already associated with an account.</param>
        /// <returns>An <see cref="ApiError"/> indicating the specified <paramref name="email"/> is already associated with an account.</returns>
        public virtual ApiError DuplicateEmail(string email)
        {
            return new ApiError
            {
                Code = nameof(DuplicateEmail),
                Description = MessageResources.FormatDuplicateEmail(email)
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating the specified <paramref name="role"/> name is invalid.
        /// </summary>
        /// <param name="role">The invalid role.</param>
        /// <returns>An <see cref="ApiError"/> indicating the specific role <paramref name="role"/> name is invalid.</returns>
        public virtual ApiError InvalidRoleName(string role)
        {
            return new ApiError
            {
                Code = nameof(InvalidRoleName),
                Description = MessageResources.FormatInvalidRoleName(role)
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating the specified <paramref name="role"/> name already exists.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="ApiError"/> indicating the specific role <paramref name="role"/> name already exists.</returns>
        public virtual ApiError DuplicateRoleName(string role)
        {
            return new ApiError
            {
                Code = nameof(DuplicateRoleName),
                Description = MessageResources.FormatDuplicateRoleName(role)
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating a user already has a password.
        /// </summary>
        /// <returns>An <see cref="ApiError"/> indicating a user already has a password.</returns>
        public virtual ApiError UserAlreadyHasPassword()
        {
            return new ApiError
            {
                Code = nameof(UserAlreadyHasPassword),
                Description = ExceptionConstant.UserAlreadyHasPassword
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating user lockout is not enabled.
        /// </summary>
        /// <returns>An <see cref="ApiError"/> indicating user lockout is not enabled.</returns>
        public virtual ApiError UserLockoutNotEnabled()
        {
            return new ApiError
            {
                Code = nameof(UserLockoutNotEnabled),
                Description = ExceptionConstant.UserLockoutNotEnabled
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating a user is already in the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="ApiError"/> indicating a user is already in the specified <paramref name="role"/>.</returns>
        public virtual ApiError UserAlreadyInRole(string role)
        {
            return new ApiError
            {
                Code = nameof(UserAlreadyInRole),
                Description = MessageResources.FormatUserAlreadyInRole(role)
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating a user is not in the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="ApiError"/> indicating a user is not in the specified <paramref name="role"/>.</returns>
        public virtual ApiError UserNotInRole(string role)
        {
            return new ApiError
            {
                Code = nameof(UserNotInRole),
                Description = MessageResources.FormatUserNotInRole(role)
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating a password of the specified <paramref name="length"/> does not meet the minimum length requirements.
        /// </summary>
        /// <param name="length">The length that is not long enough.</param>
        /// <returns>An <see cref="ApiError"/> indicating a password of the specified <paramref name="length"/> does not meet the minimum length requirements.</returns>
        public virtual ApiError PasswordTooShort(int length)
        {
            return new ApiError
            {
                Code = nameof(PasswordTooShort),
                Description = MessageResources.FormatPasswordTooShort(length)
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating a password of the specified <paramref name="length"/> does not meet the minimum length requirements.
        /// </summary>
        /// <param name="length">The length that is too long.</param>
        /// <returns>An <see cref="ApiError"/> indicating a password of the specified <paramref name="length"/> does not meet the maximum length requirements.</returns>
        public virtual ApiError PasswordTooLong(int length)
        {
            return new ApiError
            {
                Code = nameof(PasswordTooLong),
                Description = MessageResources.FormatPasswordTooLong(length)
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating a password does not meet the minimum number <paramref name="uniqueChars"/> of unique chars.
        /// </summary>
        /// <param name="uniqueChars">The number of different chars that must be used.</param>
        /// <returns>An <see cref="ApiError"/> indicating a password does not meet the minimum number <paramref name="uniqueChars"/> of unique chars.</returns>
        public virtual ApiError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new ApiError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = MessageResources.FormatPasswordRequiresUniqueChars(uniqueChars)
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating a password entered does not contain a non-alphanumeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="ApiError"/> indicating a password entered does not contain a non-alphanumeric character.</returns>
        public virtual ApiError PasswordRequiresNonAlphanumeric()
        {
            return new ApiError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = ExceptionConstant.PasswordRequiresNonAlphanumeric
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating a password entered does not contain a numeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="ApiError"/> indicating a password entered does not contain a numeric character.</returns>
        public virtual ApiError PasswordRequiresDigit()
        {
            return new ApiError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = ExceptionConstant.PasswordRequiresDigit
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating a password entered does not contain a lower case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="ApiError"/> indicating a password entered does not contain a lower case letter.</returns>
        public virtual ApiError PasswordRequiresLower()
        {
            return new ApiError
            {
                Code = nameof(PasswordRequiresLower),
                Description = ExceptionConstant.PasswordRequiresLower
            };
        }

        /// <summary>
        /// Returns an <see cref="ApiError"/> indicating a password entered does not contain an upper case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="ApiError"/> indicating a password entered does not contain an upper case letter.</returns>
        public virtual ApiError PasswordRequiresUpper()
        {
            return new ApiError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = ExceptionConstant.PasswordRequiresUpper
            };
        }

        public virtual ApiError PhotoSizeInvalid()
        {
            return new ApiError
            {
                Code = nameof(PhotoSizeInvalid),
                Description = ErrorMessageConst.PhotoSizeInvalidException
            };
        }
    }
}
