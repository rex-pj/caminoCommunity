using Coco.Api.Framework.Models;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IWorkContext
    {
        ApplicationUser CurrentUser { get; set; }
        string AuthenticationToken { get; }
}
}
