using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Api.Content.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET api/user
        [HttpGet, Route("")]
        public ActionResult<IEnumerable<string>> Index()
        {
            return new string[] { "post1", "post2" };
        }
    }
}
