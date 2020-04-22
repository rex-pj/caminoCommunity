using System;
using System.Linq;
using System.Security.Cryptography;

namespace Coco.Framework.Commons.Encode
{
    public class SaltGenerator
    {
        private static Random random = new Random();

        private static int saltLengthLimit = 50;
        public static string GetSalt()
        {
            return GetSaltText();
        }

        private static string GetSaltText()
        {
            byte[] bytes = GetSaltBytes();
            return Convert.ToBase64String(bytes);
        }

        private static byte[] GetSaltBytes()
        {
            byte[] salt = new byte[saltLengthLimit];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        private static string GeneratePlainText()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] array = Enumerable.Repeat(chars, saltLengthLimit).Select(s => s[random.Next(s.Length)]).ToArray();
            string randomString = new string(array);

            return randomString;
        }   
    }
}
