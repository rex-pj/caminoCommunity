using Coco.Api.Framework.Models;

namespace Coco.Api.Framework.UserIdentity.Contracts
{
    public interface ISessionContext
    {
        ApplicationUser CurrentUser { get; set; }
        string AuthenticationToken { get; }
}
}
