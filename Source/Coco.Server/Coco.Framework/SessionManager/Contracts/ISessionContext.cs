using Coco.Framework.Models;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface ISessionContext
    {
        ApplicationUser CurrentUser { get; set; }
        string AuthenticationToken { get; }
    }
}
