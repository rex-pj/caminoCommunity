namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface ITextCrypter
    {
        string Encrypt(string plainText, string saltKey);
        string Decrypt(string encryptedText, string saltKey);
    }
}
