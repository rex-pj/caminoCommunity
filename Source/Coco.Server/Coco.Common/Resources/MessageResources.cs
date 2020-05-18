namespace Coco.Common.Resources
{
    public class MessageResources
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

        public static string FormatInvalidRoleName(string role)
        {
            return $"Role name '{role}' is invalid.";
        }

        public static string FormatDuplicateRoleName(string role)
        {
            return $"Role name '{role}' is already taken.";
        }

        public static string FormatPasswordRequiresUniqueChars(int uniqueChars)
        {
            return $"Passwords must use at least {uniqueChars} different characters.";
        }

        public static string FormatPasswordTooShort(int length)
        {
            return $"Passwords must be at least {length} characters.";
        }

        public static string FormatPasswordTooLong(int length)
        {
            return $"Passwords must be shorter than {length} characters.";
        }

        public static string FormatUserNotInRole(string role)
        {
            return $"User is not in role '{role}'.";
        }

        public static string FormatUserAlreadyInRole(string role)
        {
            return $"User already in role '{role}'.";
        }
    }
}
