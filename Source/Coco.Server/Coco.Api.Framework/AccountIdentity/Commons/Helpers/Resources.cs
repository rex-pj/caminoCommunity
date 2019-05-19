namespace Coco.Api.Framework.AccountIdentity.Commons.Helpers
{
    public class Resources
    {
        public static string FormatInvalidUserName(string userName)
        {
            return $"User name '{userName}' is invalid, can only contain letters or digits.";
        }

        public static string FormatInvalidEmail(string userEmail)
        {
            return $"Email '{userEmail}' is invalid.";
        }

        public static string FormatDuplicateUserName(string userName)
        {
            return $"User name '{userName}' is already taken.";
        }

        public static string FormatDuplicateEmail(string userEmail)
        {
            return $"Email '{userEmail}' is already taken.";
        }

        internal static string FormatInvalidRoleName(string role)
        {
            return $"Role name '{role}' is invalid.";
        }

        internal static string FormatDuplicateRoleName(string role)
        {
            return $"Role name '{role}' is already taken.";
        }

        internal static string FormatPasswordRequiresUniqueChars(int uniqueChars)
        {
            return $"Passwords must use at least {uniqueChars} different characters.";
        }

        internal static string FormatPasswordTooShort(int length)
        {
            return $"Passwords must be at least {length} characters.";
        }

        internal static string FormatUserNotInRole(string role)
        {
            return $"User is not in role '{role}'.";
        }

        internal static string FormatUserAlreadyInRole(string role)
        {
            return $"User already in role '{role}'.";
        }
    }
}
