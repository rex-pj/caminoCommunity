using Camino.Framework.SessionManager.Contracts;
using Camino.Core.Utils;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Camino.Framework.Models;
using Microsoft.Extensions.Options;

namespace Camino.Framework.SessionManager
{
    public class TextEncryption : ITextEncryption
    {
        private readonly CrypterSettings _crypterSettings;

        public TextEncryption(IOptions<CrypterSettings> crypterSettings)
        {
            _crypterSettings = crypterSettings.Value;
        }

        public string Encrypt(string plainText, string saltKey)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var bytesDerived = new Rfc2898DeriveBytes(_crypterSettings.PepperKey, Encoding.ASCII.GetBytes(saltKey)))
            {
                var keyBytes = bytesDerived.GetBytes(256 / 8);
                using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros })
                {
                    var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(_crypterSettings.SecretKey));

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

                    return ConvertUtil.BytesToString(cipherTextBytes);
                }
            }
        }

        public string Decrypt(string encryptedText, string saltKey)
        {
            var cipherTextBytes = ConvertUtil.StringToBytes(encryptedText);
            using (var bytesDerived = new Rfc2898DeriveBytes(_crypterSettings.PepperKey, Encoding.ASCII.GetBytes(saltKey)))
            {
                var keyBytes = bytesDerived.GetBytes(256 / 8);
                using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros })
                {
                    var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(_crypterSettings.SecretKey));
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
