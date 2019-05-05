using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Content.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        // GET api/user
        [HttpGet, Route("")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "post1", "post2" };
        }
    }
}
