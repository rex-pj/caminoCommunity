using Coco.Framework.Attributes;
using Microsoft.AspNetCore.Http;

namespace Coco.Framework.Controllers
{
    [ApplicationAuthentication]
    public class BaseAuthController : BaseController
    {
        public BaseAuthController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }
}
