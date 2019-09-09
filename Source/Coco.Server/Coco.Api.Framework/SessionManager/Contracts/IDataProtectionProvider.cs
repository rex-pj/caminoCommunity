namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface IDataProtectionProvider
    {
        IDataProtector CreateProtector(string purpose);
    }
}
