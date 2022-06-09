namespace Camino.Infrastructure.Identity.Interfaces
{
    public interface ITextEncryption
    {
        string Encrypt(string plainText, string saltKey);
        string Decrypt(string encryptedText, string saltKey);
    }
}
