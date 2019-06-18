using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.Commons.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Coco.Api.Framework.AccountIdentity
{
    public class TextCrypter : ITextCrypter
    {
        private readonly string _pepperKey;
        private readonly string _vIKey;

        public TextCrypter(IConfiguration configuration)
        {
            _pepperKey = configuration["Crypter:PepperKey"];
            _vIKey = configuration.GetValue<string>("Crypter:SecretKey");
        }

        public string Encrypt(string plainText, string saltKey)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var bytesDerived = new Rfc2898DeriveBytes(_pepperKey, Encoding.ASCII.GetBytes(saltKey)))
            {
                var keyBytes = bytesDerived.GetBytes(256 / 8);
                using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros })
                {
                    var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(_vIKey));

                    byte[] cipherTextBytes;

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            cipherTextBytes = memoryStream.ToArray();
                            cryptoStream.Close();
                        }
                        memoryStream.Close();
                    }

                    return DataConverter.BytesToString(cipherTextBytes);
                }
            }
        }

        public string Decrypt(string encryptedText, string saltKey)
        {
            var cipherTextBytes = DataConverter.StringToBytes(encryptedText);
            using (var bytesDerived = new Rfc2898DeriveBytes(_pepperKey, Encoding.ASCII.GetBytes(saltKey)))
            {
                var keyBytes = bytesDerived.GetBytes(256 / 8);
                using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros })
                {
                    var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(_vIKey));
                    var plainText = string.Empty;
                    using (var memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            var plainTextBytes = new byte[cipherTextBytes.Length];

                            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.Close();
                            plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
                        }
                        memoryStream.Close();
                    }

                    return plainText;
                }
            }
        }
    }
}
