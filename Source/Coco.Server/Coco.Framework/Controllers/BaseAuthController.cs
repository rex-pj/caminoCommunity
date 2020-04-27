using Coco.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Coco.Framework.Controllers
{
    [AuthenticationSession]
    public class BaseAuthController : Controller
    {
    }
}
