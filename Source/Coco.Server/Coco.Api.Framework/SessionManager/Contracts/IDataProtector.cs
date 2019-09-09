namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IDataProtector : IDataProtectionProvider
    {
        byte[] Protect(byte[] plaintext);
        byte[] Unprotect(byte[] protectedData);
    }
}
