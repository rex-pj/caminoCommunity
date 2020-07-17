using Camino.Framework.Attributes;
using Microsoft.AspNetCore.Http;

namespace Camino.Framework.Controllers
{
    [ApplicationAuthentication]
    public class BaseAuthController : BaseController
    {
        public BaseAuthController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }
}
