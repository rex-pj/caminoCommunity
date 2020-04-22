namespace Coco.Framework.Commons.Constants
{
    public class ExceptionConstant
    {
        public static readonly string InvalidPasswordHasherCompatibilityMode = "Invalid Password Hasher CompatibilityMode";
        public static readonly string InvalidPasswordHasherIterationCount = "Invalid Password Hasher IterationCount";
        public static readonly string StoreNotIUserLockoutStore = "Store Not IUserLockoutStore";
        public static readonly string StoreNotIUserSecurityStampStore = "Store Not IUserSecurityStampStore";
        public static readonly string StoreNotIUserPasswordStore = "Store Not IUserPasswordStore";
        public static readonly string NullSecurityStamp = "Null SecurityStamp";
        public static readonly string ConcurrencyFailure = "Concurrency Failure";
        public static readonly string DefaultError = "Default Error";
        internal static readonly string LoginAlreadyAssociated = "Login Already Associated";
        internal static readonly string InvalidToken = "Invalid Token";
        internal static readonly string UserAlreadyHasPassword = "User Already Has Password";
        internal static readonly string UserAlreadyActived = "User Already Actived";
        internal static readonly string UserLockoutNotEnabled = "User Lockout Not Enabled";
        public static readonly string PasswordMismatch = "Password Mismatch";
        public static readonly string RecoveryCodeRedemptionFailed = "Recovery Code Redemption Failed";
        internal static readonly string PasswordRequiresNonAlphanumeric = "Password Requires Non Alphanumeric";
        internal static readonly string PasswordRequiresDigit = "Password Requires Digit";
        internal static readonly string PasswordRequiresLower = "Password Requires Lower";
        internal static readonly string PasswordRequiresUpper = "Password Requires Upper";
        public static readonly string UnexpectedErrorOccurred = "An unexpected error has occurred";
    }
}
