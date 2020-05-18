using Coco.Framework.SessionManager.Contracts;
using System;
using System.Security.Cryptography;

namespace Coco.Framework.SessionManager
{
    public class TextRandom : ITextRandom
    {
        private readonly Random random = new Random();

        private readonly int _saltLengthLimit = 50;
        
        public string GetSalt()
        {
            return GetSaltText();
        }

        private string GetSaltText()
        {
            byte[] bytes = GetSaltBytes();
            return Convert.ToBase64String(bytes);
        }

        private byte[] GetSaltBytes()
        {
            byte[] salt = new byte[_saltLengthLimit];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }
    }
}
