using System;

namespace Coco.Api.Framework.AccountIdentity.Commons.Helpers
{
    public class Resources
    {
        public static string FormatInvalidUserName(string userName)
        {
            return userName;
        }

        public static string FormatInvalidEmail(string userName)
        {
            return userName;
        }

        public static string FormatDuplicateUserName(string userName)
        {
            return userName;
        }

        public static string FormatDuplicateEmail(string userName)
        {
            return userName;
        }

        internal static string FormatInvalidRoleName(string role)
        {
            throw new NotImplementedException();
        }

        internal static string FormatDuplicateRoleName(string role)
        {
            throw new NotImplementedException();
        }

        internal static string FormatPasswordRequiresUniqueChars(int uniqueChars)
        {
            throw new NotImplementedException();
        }

        internal static string FormatPasswordTooShort(int length)
        {
            throw new NotImplementedException();
        }

        internal static string FormatUserNotInRole(string role)
        {
            throw new NotImplementedException();
        }

        internal static string FormatUserAlreadyInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}
