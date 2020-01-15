using Coco.Api.Framework.Models;

namespace Coco.Api.Framework.SessionManager.Contracts
{
    public interface ISessionContext
    {
        ApplicationUser CurrentUser { get; set; }
        string AuthenticationToken { get; }
    }
}
